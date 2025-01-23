using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using ApiPyme.Service;
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
        private readonly EmailService _emailService;
        public UsuarioRepositoryImpl(AppDbContext context, IUsuarioRolRepository usuarioRolRepository, 
            IRolRepository rolRepository)
        {
            _context = context;
            _usuarioRolRepository = usuarioRolRepository;
            _rolRepository = rolRepository;
            _emailService = new EmailService();
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
                // Verificar si el usuario ya existe en la base de datos por su identificación
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Identificacion == usuarioDto.Identificacion && u.Activo == true);

                if (usuarioExistente != null)
                {
                    // Retornar false o lanzar una excepción indicando que el usuario ya existe
                    throw new Exception("El usuario con esta identificación ya existe.");
                }

                // Crear un nuevo usuario si no existe
                Usuario nuevoUsuario = new Usuario
                {
                    Identificacion = usuarioDto.Identificacion,
                    Nombres = usuarioDto.Nombres,
                    Apellidos = usuarioDto.Apellidos,
                    Direccion = usuarioDto.Direccion,
                    Telefono = usuarioDto.Telefono,
                    Password = usuarioDto.Password,
                    Mail = usuarioDto.Mail,
                    Activo = true
                };

                // Obtener el rol asociado al usuario
                Rol rol = await _rolRepository.GetRolByNombre(usuarioDto.NombreRol);
                if (rol == null)
                {
                    throw new Exception("El rol especificado no existe.");
                }

                // Crear la relación usuario-rol
                UsuarioRol usuarioRol = new UsuarioRol
                {
                    usuario = nuevoUsuario,
                    rol = rol
                };

                // Crear un hash para la contraseña del usuario
                nuevoUsuario.Password = _passwordHasher.HashPassword(nuevoUsuario, nuevoUsuario.Password);

                // Agregar el usuario y su relación con el rol a la base de datos
                await _context.Usuarios.AddAsync(nuevoUsuario);
                await _context.UsuarioRol.AddAsync(usuarioRol);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
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

                    if (!string.IsNullOrEmpty(usuario.Password))
                    {
                        existe.Password = _passwordHasher.HashPassword(existe, usuario.Password);
                        passwordModificado(existe.Mail);
                    }
                    existe.Telefono = string.IsNullOrEmpty(usuario.Telefono) ? existe.Telefono : usuario.Telefono;
                    existe.Mail = string.IsNullOrEmpty(usuario.Mail) ? existe.Mail : usuario.Mail;
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
                Mail = u.Mail,
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
                                 .FirstOrDefaultAsync(u => u.Identificacion == identificacion && u.Activo == true);

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
                usuario.Mail = user.Mail;
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

        public async Task<UsuarioDto> GetUsuarioById(int id)
        {
            try
            {
                // Buscar el usuario por identificación
                var user = await _context.Usuarios
                                 .Include(u => u.UsuarioRoles)
                                 .ThenInclude(ur => ur.rol)
                                 .FirstOrDefaultAsync(u => u.IdUsuario == id && u.Activo == true);

                // Si no se encuentra el usuario, lanzar una excepción
                if (user == null)
                {
                    throw new KeyNotFoundException($"Usuario no encontrado.");
                }
                // Devolver el usuario encontrado
                UsuarioDto usuario = new UsuarioDto();
                usuario.IdUsuario = user.IdUsuario;
                usuario.Nombres = user.Nombres + " " + user.Apellidos;
                usuario.Identificacion = user.Identificacion;
                usuario.Direccion = user.Direccion;
                usuario.Mail = user.Mail;
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
                Mail = u.Mail,
                CreatedAt = u.CreatedAt
            }).ToList();

            return new PagedResult<UsuarioDto>
            {
                TotalCount = totalCount,
                Items = usuarioDTOs
            };
        }

        public async Task<bool> RecoveryPassword(EmailDto correo)
        {

            try
            {
                var user = await _context.Usuarios
                                 .Include(u => u.UsuarioRoles)
                                 .ThenInclude(ur => ur.rol)
                                 .FirstOrDefaultAsync(u => u.Mail == correo.Correo && u.Activo == true);
                if (user == null)
                {
                    throw new Exception("El correo no es valido");
                }

                var token = Guid.NewGuid().ToString();
                var fechaExpiracion = DateTime.UtcNow.AddHours(1);

                RecuperacionPassword recuperacion = new RecuperacionPassword();
                recuperacion.UsuarioId = user.IdUsuario;
                recuperacion.Token = token;
                recuperacion.FechaExpiracion = fechaExpiracion;

                await _context.RecuperacionPassword.AddAsync(recuperacion);
                await _context.SaveChangesAsync();

                // Enviar correo con el enlace
                var enlace = $"http://localhost:4200/#/recovery?token={token}";
                _emailService.sendMail(correo.Correo, "Recuperación de Contraseña",
                    $"Hola,<br><br>Haz solicitado recuperar tu contraseña. " +
                    $"Haz clic en el siguiente enlace para restablecerla:<br><a href='{enlace}'>Restablecer Contraseña</a><br><br>Este enlace es válido por 1 hora.<br><br>Gracias.");

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> UpdatePassword(RecuperarPasswordDTO recuperarPasswordDTO)
        {
            try
            {
                var recuperacion = await _context.RecuperacionPassword
               .FirstOrDefaultAsync(r => r.Token == recuperarPasswordDTO.Token && r.FechaExpiracion > DateTime.UtcNow);

                if (recuperacion == null)
                {
                    throw new Exception("El token es inválido o ha expirado.");
                }

                var usuario = await _context.Usuarios.FindAsync(recuperacion.UsuarioId);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                usuario.Password = _passwordHasher.HashPassword(usuario, recuperarPasswordDTO.Password);

                // Guardar los cambios y eliminar el token
                _context.RecuperacionPassword.Remove(recuperacion);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> passwordModificado(String correo) {

            try
            {
                _emailService.sendMail(correo, "Cambio de Contraseña",
                    $"Hola,<br><br>Haz cambiado la contraseña de tu cuenta, sino realizaste esta acción por favor ve al apartado de login y recuperar contraseña para reestablecerla. ");
                return true;
            } catch (Exception ex) {
                throw;
            }
            
        }
    }
}
