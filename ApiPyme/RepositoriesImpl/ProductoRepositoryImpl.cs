using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace ApiPyme.RepositoriesImpl
{
    public class ProductoRepositoryImpl : IProductoRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        public ProductoRepositoryImpl(AppDbContext context, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }
        public Task<bool> DeleteProducto(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateQR(Producto producto)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{producto.IdProducto}-{producto.NombreProducto}", QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                // Definir la ruta donde se guardará la imagen
                string imagePath = Path.Combine("wwwroot/images/qrcodes", $"{producto.IdProducto}.png");
                string fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

                // Guardar la imagen en el servidor
                await File.WriteAllBytesAsync(fullImagePath, qrCodeImage);

                // Retornar la ruta relativa de la imagen
                return imagePath;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante la escritura de archivos
                throw new InvalidOperationException("Error al guardar la imagen QR", ex);
            }
        }

        public async Task<IEnumerable<Producto>> GetAllProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> GetProducto(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<bool> SaveProducto(ProductoDto productoDto)
        {
            try
            {
                if (string.IsNullOrEmpty(productoDto.IdProveedor))
                {
                    throw new NotImplementedException("Proveedor no asignado");
                }
                // consulto el usuario para guardarlo junto al producto
                Usuario proveedor = await _usuarioRepository.GetUsuario(Int32.Parse(productoDto.IdProveedor));
                Producto producto = new Producto
                {
                    NombreProducto = productoDto.NombreProducto,
                    Stock = Int32.Parse(productoDto.Stock),
                    Precio = decimal.Parse(productoDto.Precio),
                    Descripcion = productoDto.Descripcion,
                    NombreCategoria = productoDto.NombreCategoria,
                    Observacion = productoDto.Observacion,
                    usuarioProveedor = proveedor
                };
                // Guardar el producto para que se genere el IdProducto
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();
                // Generar la imagen QR con el IdProducto
                var qrPath = await GenerateQR(producto);
                if (string.IsNullOrEmpty(qrPath))
                {
                    throw new InvalidOperationException("No se pudo generar el código QR.");
                }
                // Actualizar el producto con la ruta del QR
                producto.QrPath = qrPath;
                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<Producto> SaveProductoCompra(ProductoDto productoDto)
        {
            try
            {
                if (string.IsNullOrEmpty(productoDto.IdProveedor))
                {
                    throw new NotImplementedException("Proveedor no asignado");
                }
                // consulto el usuario para guardarlo junto al producto
                Usuario proveedor = await _usuarioRepository.GetUsuario(Int32.Parse(productoDto.IdProveedor));
                Producto producto = new Producto
                {
                    NombreProducto = productoDto.NombreProducto,
                    Stock = Int32.Parse(productoDto.Stock),
                    Precio = decimal.Parse(productoDto.Precio),
                    Descripcion = productoDto.Descripcion,
                    Observacion = productoDto.Observacion,
                    usuarioProveedor = proveedor
                };
                // Guardar el producto para que se genere el IdProducto
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();
                // Generar la imagen QR con el IdProducto
                var qrPath = await GenerateQR(producto);
                if (string.IsNullOrEmpty(qrPath))
                {
                    throw new InvalidOperationException("No se pudo generar el código QR.");
                }
                // Actualizar el producto con la ruta del QR
                producto.QrPath = qrPath;
                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return producto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateProducto(Producto producto)
        {
            try
            {
                var existe = await _context.Productos.FindAsync(producto.IdProducto);
                if (existe != null)
                {
                    existe.Stock = producto.Stock;
                    existe.Observacion = producto.Observacion;
                    existe.NombreProducto = producto.NombreProducto;
                    existe.NombreCategoria = producto.NombreCategoria;
                    existe.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }

        public async Task<bool> UpdateProductoCompra(Producto producto)
        {
            try
            {
                var existe = await _context.Productos.FindAsync(producto.IdProducto);
                if (existe != null)
                {
                    existe.Stock = producto.Stock;
                    existe.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // Manejo de excepciones
                return false;
            }
        }

        public async Task<Producto> GetProductoEstado(int id, string estadoProducto)
        {
            return await _context.Productos
                    .FirstOrDefaultAsync(p => p.IdProducto == id && p.EstadoProducto == estadoProducto);
        }

        public async Task<PagedResult<ProductoDto>> GetProductosByUsuario(int page, int size, string search) {
            var query = from p in _context.Productos
                        join u in _context.Usuarios on p.IdUsuarioProveedor equals u.IdUsuario
                        select new
                        {
                            p,
                            UsuarioId = u.IdUsuario,
                            UsuarioNombre = u.Nombres,
                            UsuarioApellido = u.Apellidos
                        };

            if (!string.IsNullOrEmpty(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(u => u.p.NombreProducto.ToLower().Contains(searchLower));
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

            var productoDto = items.Select(p => new ProductoDto
            {
                IdProducto = p.p.IdProducto.ToString(),
                NombreProducto = p.p.NombreProducto,
                Descripcion = p.p.Descripcion,
                NombreCategoria = p.p.NombreCategoria,
                Precio = p.p.Precio.ToString("F2", CultureInfo.InvariantCulture),
                Stock = p.p.Stock + "",
                Activo = p.p.Activo,
                CreatedAt = p.p.CreatedAt,
                IdProveedor = p.UsuarioId.ToString(),
                Proveedor = p.UsuarioNombre + " " + p.UsuarioApellido
            });

            // Retornar el resultado paginado
            return new PagedResult<ProductoDto>
            {
                TotalCount = totalCount,
                Items = productoDto
            };
        }

        public async Task<PagedResult<ProductoDto>> GetProductosByProveedor(int page, int size, string identificacion, string search)
        {
            // Construir la consulta inicial con el join y el filtro por identificación
            var query = _context.Productos
                                .Include(p => p.usuarioProveedor)
                                .Where(p => p.usuarioProveedor.Identificacion == identificacion);

            // Aplicar el filtro por búsqueda si el parámetro search no está vacío
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.NombreProducto.Contains(search));
            }

            // Asegura que los valores de paginación sean válidos
            if (page < 1)
                page = 1;

            if (size < 1)
                size = 10; // Establecer un tamaño predeterminado si no es válido

            // Total de elementos antes de aplicar paginación
            int totalCount = await query.CountAsync();

            // Calcular el número de elementos a saltar
            int skip = (page - 1) * size;

            // Obtener los elementos de la página actual
            var items = await query.Skip(skip).Take(size).ToListAsync();

            // Proyectar los productos a ProductoDto
            var productoDto = items.Select(p => new ProductoDto
            {
                IdProducto = p.IdProducto + "",
                NombreProducto = p.NombreProducto,
                Descripcion = p.Descripcion,
                Observacion = p.Observacion,
                Precio = p.Precio.ToString(),
                Stock = p.Stock.ToString(),
                Activo = p.Activo,
                CreatedAt = p.CreatedAt
            });

            // Retornar el resultado paginado
            return new PagedResult<ProductoDto>
            {
                TotalCount = totalCount,
                Items = productoDto
            };
        }

        public async Task<PagedResult<ProductoDto>> GetProductosCliente(int page, int size, string search)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.NombreProducto.Contains(search));
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

            var productoDto = items.Select(p => new ProductoDto
            {
                IdProducto = p.IdProducto.ToString(),
                NombreProducto = p.NombreProducto,
                Descripcion = p.Descripcion,
                Observacion = p.Observacion,
                Precio = p.Precio + "",
                Stock = p.Stock + "",
                Activo = p.Activo,
                CreatedAt = p.CreatedAt
            });

            // Retornar el resultado paginado
            return new PagedResult<ProductoDto>
            {
                TotalCount = totalCount,
                Items = productoDto
            };
        }

        public async Task<ActionResult<ProductoDto>> GetProductoConQr(int idProducto)
        {
            // Obtén el producto por su Id
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            if (producto == null)
            {
                throw new ("Producto no encontrado.");
            }

            // Suponiendo que el campo QrPath tiene la ruta de la imagen
            var qrImagePath = Path.Combine(Directory.GetCurrentDirectory(), producto.QrPath);

            string qrImageBase64 = null;

            // Verifica si el archivo existe en la ruta especificada
            if (System.IO.File.Exists(qrImagePath))
            {
                try
                {
                    // Lee los bytes del archivo
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(qrImagePath);

                    // Asegúrate de que los bytes se han leído correctamente
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        // Convierte los bytes a base64
                        qrImageBase64 = Convert.ToBase64String(imageBytes);
                    }
                    else
                    {
                        // Si no se leen bytes, muestra un mensaje de error
                        Console.WriteLine("No se pudieron leer los bytes de la imagen.");
                    }
                }
                catch (Exception ex)
                {
                    // Muestra el mensaje de error si ocurre una excepción
                    Console.WriteLine($"Error al leer la imagen: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe en la ruta especificada.");
            }

            // Crea un DTO que incluya la información del producto y el QR
            var productoDto = new ProductoDto
            {
                NombreProducto = producto.NombreProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio.ToString(),
                // Incluye la imagen del QR (puede ser en base64 o una URL)
                QrCodeImage = qrImageBase64
            };

            return productoDto;
        }

        public async Task<bool> UpdateProductoDto(ProductoDto productoDto)
        {
            try
            {
                var existe = await _context.Productos.FindAsync(Int32.Parse(productoDto.IdProducto));
                if (existe != null)
                {
                    existe.Stock = Int32.Parse(productoDto.Stock);
                    existe.Observacion = productoDto.Observacion;
                    existe.NombreProducto = productoDto.NombreProducto;
                    existe.NombreCategoria = productoDto.NombreCategoria;
                    existe.Precio = decimal.Parse(productoDto.Precio);
                    existe.UpdateAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}