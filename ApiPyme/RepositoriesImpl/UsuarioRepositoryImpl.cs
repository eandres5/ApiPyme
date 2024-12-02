using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;

namespace ApiPyme.RepositoriesImpl
{
    public class UsuarioRepositoryImpl : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();
        private readonly IRolRepository _rolRepository;
        private readonly IUsuarioRolRepository _usuarioRolRepository;
        public UsuarioRepositoryImpl(AppDbContext context, IUsuarioRolRepository usuarioRolRepository, IRolRepository rolRepository)
        {
            _context = context;
            _usuarioRolRepository = usuarioRolRepository;
            _rolRepository = rolRepository;
        }
        public async Task<bool> DeleteUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                Usuario usuario = new Usuario();
                usuario = await GetUsuario(usuarioDto.IdUsuario);
                usuario.Activo = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }

        public async Task<PagedResult<UsuarioDto>> GetAllUsuarios(int page, int size, string search)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Identificacion.Contains(search));
            }

            // Asegura que los valores de paginación sean válidos
            if (page < 1)
                page = 1;

            if (size < 1)
                size = 10; // Establecer un tamaño predeterminado si no es válido

            // Calcula el número de elementos a saltar
            int skip = (page - 1) * size;

            // Total de elementos antes de aplicar paginación
            int totalCount = await query.CountAsync();

            // Obtener los elementos de la página actual
            var items = await query.Skip(skip).Take(size).ToListAsync();

            var usuarioDTOs = items.Select(u => new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Identificacion = u.Identificacion,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Activo = u.Activo,
                CreatedAt = u.CreatedAt
            });

            // Retornar el resultado paginado
            return new PagedResult<UsuarioDto>
            {
                TotalCount = totalCount,
                Items = usuarioDTOs
            };
        }

        public async Task<Usuario> GetLogin(LoginRequestDto login)
        {
            var usuarioLogin = await GetUsurioIdentificacionRoles(login.Identificacion);
            if (usuarioLogin == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(usuarioLogin, usuarioLogin.Password, login.Password);

            // Verificar si la contraseña es correcta
            if (result == PasswordVerificationResult.Success)
            {
                return usuarioLogin;
            }
            return null;
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<bool> SaveUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.Identificacion = usuarioDto.Identificacion;
                nuevoUsuario.Nombres = usuarioDto.Nombres;
                nuevoUsuario.Apellidos = usuarioDto.Apellidos;
                nuevoUsuario.Direccion = usuarioDto.Direccion;
                nuevoUsuario.Telefono = usuarioDto.Telefono;
                nuevoUsuario.Password = usuarioDto.Password;
                nuevoUsuario.Activo = true;
                // se obtiene el rol para el usuario
                Rol rol = await _rolRepository.GetRolByNombre(usuarioDto.NombreRol);
                // se crea la relacion usario rol 
                UsuarioRol usuarioRol = new UsuarioRol();
                usuarioRol.usuario = nuevoUsuario;
                usuarioRol.rol = rol;
                // se crea un hash para la contraseña del usuario
                nuevoUsuario.Password = _passwordHasher.HashPassword(nuevoUsuario, nuevoUsuario.Password);

                await _context.Usuarios.AddAsync(nuevoUsuario);
                await _context.UsuarioRol.AddAsync(usuarioRol);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }

        public async Task<bool> UpdateUsuario(UsuarioDto usuario)
        {
            try
            {
                var existe = await _context.Usuarios.FindAsync(usuario.IdUsuario);
                if (existe != null)
                {
                    existe.Nombres = string.IsNullOrEmpty(usuario.Nombres) ? existe.Nombres : usuario.Nombres;
                    existe.Apellidos = string.IsNullOrEmpty(usuario.Apellidos) ? existe.Apellidos : usuario.Apellidos;
                    existe.Identificacion = string.IsNullOrEmpty(usuario.Identificacion) ? existe.Identificacion : usuario.Identificacion;
                    existe.Direccion = string.IsNullOrEmpty(usuario.Direccion) ? existe.Direccion : usuario.Direccion;
                    existe.Password = string.IsNullOrEmpty(usuario.Password) ? existe.Password : usuario.Password;
                    existe.Telefono = string.IsNullOrEmpty(usuario.Telefono) ? existe.Telefono : usuario.Telefono;
                    existe.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Usuario> GetUsurioIdentificacionRoles(string identification)
        {
            try
            {
                // Buscar el usuario por identificación
                var user = await _context.Usuarios
                                 .Include(u => u.UsuarioRoles)
                                 .ThenInclude(ur => ur.rol)
                                 .FirstOrDefaultAsync(u => u.Identificacion == identification);

                // Si no se encuentra el usuario, lanzar una excepción
                if (user == null)
                {
                    throw new KeyNotFoundException($"Usuario con identificación '{identification}' no encontrado.");
                }
                // Devolver el usuario encontrado
                return user;
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar la excepción o relanzarla
                // Ejemplo: loguear la excepción
                Console.WriteLine($"Error al obtener el usuario: {ex.Message}");
                throw;
            }

        }

        public async Task<PagedResult<UsuarioDto>> GetAllUsuariosProveedor(int page, int size, string search)
        {
            var query = from u in _context.Usuarios
            join ur in _context.UsuarioRoles on u.IdUsuario equals ur.IdUsuario
            join r in _context.Rols on ur.IdRol equals r.IdRol
            where u.Activo == true && r.Nombre == "PROVEEDOR"
                        select u;

            // Aplica el filtro de búsqueda si el parámetro `search` no está vacío o nulo
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Identificacion.Contains(search));
            }

            // Asegura que los valores de paginación sean válidos
            if (page < 1)
                page = 1;

            if (size < 1)
                size = 10; // Establecer un tamaño predeterminado si no es válido

            // Calcula el número de elementos a saltar
            int skip = (page - 1) * size;

            // Total de elementos antes de aplicar paginación
            int totalCount = await query.CountAsync();

            // Obtener los elementos de la página actual
            var items = await query.Skip(skip).Take(size).ToListAsync();

            var usuarioDTOs = items.Select(u => new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres + " " + u.Apellidos,
                Apellidos = u.Apellidos,
                Identificacion = u.Identificacion,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Activo = u.Activo,
                CreatedAt = u.CreatedAt
            }).ToList();

            // Retornar el resultado paginado
            return new PagedResult<UsuarioDto>
            {
                TotalCount = totalCount,
                Items = usuarioDTOs
            };
        }

        public async Task<UsuarioDto> GetUsuarioByIdentificaion(string identificacion)
        {
            try
            {
                // Buscar el usuario por identificación
                var user = await _context.Usuarios
                                 .Include(u => u.UsuarioRoles)
                                 .ThenInclude(ur => ur.rol)
                                 .FirstOrDefaultAsync(u => u.Identificacion == identificacion);

                // Si no se encuentra el usuario, lanzar una excepción
                if (user == null)
                {
                    throw new KeyNotFoundException($"Usuario con identificación '{identificacion}' no encontrado.");
                }
                // Devolver el usuario encontrado
                UsuarioDto usuario = new UsuarioDto();
                usuario.IdUsuario = user.IdUsuario;
                usuario.Nombres = user.Nombres + " " + user.Apellidos;
                usuario.Identificacion = user.Identificacion;
                usuario.Direccion = user.Direccion;
                return usuario;
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar la excepción o relanzarla
                // Ejemplo: loguear la excepción
                Console.WriteLine($"Error al obtener el usuario: {ex.Message}");
                throw;
            }
        }
        public async Task<PagedResult<UsuarioDto>> GetAllUsuariosClientes(int page, int size, string search)
        {
            var query = from u in _context.Usuarios
                        join ur in _context.UsuarioRoles on u.IdUsuario equals ur.IdUsuario
                        join r in _context.Rols on ur.IdRol equals r.IdRol
                        where u.Activo == true && r.Nombre == "CLIENTE"
                        select u;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Identificacion.Contains(search));
            }

            if (page < 1)
                page = 1;

            if (size < 1)
                size = 10;

            int skip = (page - 1) * size;
            int totalCount = await query.CountAsync();
            var items = await query.Skip(skip).Take(size).ToListAsync();

            var usuarioDTOs = items.Select(u => new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres + " " + u.Apellidos,
                Apellidos = u.Apellidos,
                Identificacion = u.Identificacion,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Activo = u.Activo,
                CreatedAt = u.CreatedAt
            }).ToList();

            return new PagedResult<UsuarioDto>
            {
                TotalCount = totalCount,
                Items = usuarioDTOs
            };
        }

    }
}
