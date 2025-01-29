using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ApiPyme.RepositoriesImpl
{
    public class ComprobanteRepositoryImpl : IComprobanteRepository
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public ComprobanteRepositoryImpl(AppDbContext context, IProductoRepository productoRepository, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
            
        }

        public async Task<PagedResult<ComprobanteDto>> GetAllVentas(int page, int size, string search)
        {
            // Realiza el join entre Comprobante y Usuario usando IdUsuarioCliente
            var query = from comprobante in _context.Comprobantes
                        join usuario in _context.Usuarios
                        on comprobante.IdUsuarioCliente equals usuario.IdUsuario
                        where comprobante.TipoTransaccion.Equals("VENTA") // Filtra por tipo de transacción
                        select new
                        {
                            Comprobante = comprobante,
                            Usuario = usuario
                        };

            // Filtra los resultados si hay una búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Comprobante.NumeroComprobante.Contains(search) ||
                                         x.Usuario.Identificacion.Contains(search));
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

            // Mapear los elementos a ComprobanteDto
            var comprobanteDto = items.Select(x => new ComprobanteDto
            {
                IdComprobante = x.Comprobante.IdComprobante.ToString(),
                NumeroComprobante = x.Comprobante.NumeroComprobante,
                FechaEmision = x.Comprobante.FechaEmision.ToString("yyyy-MM-dd"), // Formato de fecha
                Total = x.Comprobante.Total.ToString("F2"),
                Iva = x.Comprobante.Iva.ToString("F2"),
                UsuarioClienteNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos // O cualquier otra propiedad relevante
            }).ToList();

            // Retornar el resultado paginado
            return new PagedResult<ComprobanteDto>
            {
                TotalCount = totalCount,
                Items = comprobanteDto
            };
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ComprobanteDto>> getAllComprobantes(int page, int size, string search)
        {
            throw new NotImplementedException();
        }

        public async Task<ComprobanteDto> GetComprobante(int id, string tipoTransacion)
        {
            var comprobante = await _context.Comprobantes
         .Include(c => c.detallesComprobante)
         .Include(c => c.usuarioCliente)
         .Include(c => c.usuarioComerciante)
         .FirstOrDefaultAsync(c => c.IdComprobante == id && c.TipoTransaccion == tipoTransacion);

            if (comprobante == null)
            {
                return null;
            }

            // Leer y convertir el archivo PDF a Base64
            string? fileBase64 = null;
            if (!string.IsNullOrEmpty(comprobante.PathPdf) && File.Exists(comprobante.PathPdf))
            {
                var fileBytes = await File.ReadAllBytesAsync(comprobante.PathPdf);
                fileBase64 = Convert.ToBase64String(fileBytes);
            }


            var comprobanteDto = new ComprobanteDto
            {
                IdComprobante = comprobante.IdComprobante.ToString(),
                NumeroComprobante = comprobante.NumeroComprobante,
                TipoComprobante = comprobante.TipoComprobante,
                TipoTransaccion = comprobante.TipoTransaccion,
                FechaEmision = comprobante.FechaEmision.ToString("yyyy-MM-dd"), // Formato de fecha
                Subtotal = comprobante.Subtotal.ToString("F2"), // Formato decimal con dos decimales
                Total = comprobante.Total.ToString("F2"),
                Iva = comprobante.Iva.ToString(),
                IdUsuarioCliente = comprobante.IdUsuarioCliente?.ToString(),
                IdUsuarioComerciante = comprobante.IdUsuarioComerciante?.ToString(),
                NombreCliente = comprobante.usuarioCliente?.Nombres + " " + comprobante.usuarioCliente?.Apellidos,
                Identificacion = comprobante.usuarioCliente?.Identificacion?.ToString(),
                Direccion = comprobante.usuarioCliente?.Direccion?.ToString(),
                UsuarioClienteNombre = comprobante.usuarioCliente?.Nombres + " " + comprobante.usuarioCliente?.Apellidos,
                FileBase64 = fileBase64
            };

            return comprobanteDto;
        }

        public async Task<bool> Save(ComprobanteDto comprobanteDto)
        {
            try
            {
                // busco el id de usuario proveedor y comerciante
                var usuarioCliente = await _usuarioRepository.GetUsuario(Int32.Parse(comprobanteDto.IdUsuarioCliente));
                var usuarioComerciante = await _usuarioRepository.GetUsuario(Int32.Parse(comprobanteDto.IdUsuarioComerciante));
                //  guardo la cabecera

                int numeroComprobante = await GetUltimoNumeroComprobante(comprobanteDto.TipoComprobante);

                Comprobante comprobante = new Comprobante();
                comprobante.NumeroComprobante = numeroComprobante.ToString();
                comprobante.TipoComprobante = comprobanteDto.TipoComprobante;
                comprobante.usuarioCliente = usuarioCliente;
                comprobante.usuarioComerciante = usuarioComerciante;
                comprobante.TipoTransaccion = comprobanteDto.TipoTransaccion;
                comprobante.FechaEmision = DateTime.ParseExact(comprobanteDto.FechaEmision, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                comprobante.Iva = Int32.Parse(comprobanteDto.Iva);
                comprobante.Subtotal = decimal.Parse(comprobanteDto.Subtotal, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                comprobante.Total = decimal.Parse(comprobanteDto.Total, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                comprobante.TipoPago = comprobanteDto.TipoPago;
                comprobante.PathPdf = "";
                //  guardo la cabecera
                if (comprobanteDto.FileBase64 != "")
                {
                    string filePath = GuardarArchivoPdf(comprobanteDto.FileBase64);
                    comprobante.PathPdf = filePath;
                }

                // primero se verifica los productos si tienen IdProducto
                await _context.Comprobantes.AddAsync(comprobante);
                await _context.SaveChangesAsync();

                foreach (var item in comprobanteDto.DetalleComprobantes)
                {
                    // Se busca el producto
                    Producto productoUpdate = await _productoRepository.GetProducto(Int32.Parse(item.IdProducto));

                    // Determina si la transacción es una "VENTA" o una "COMPRA" y ajusta el stock en consecuencia
                    int cantidad = Int32.Parse(item.Cantidad);
                    int nuevoStock = comprobanteDto.TipoTransaccion.Equals("VENTA")
                        ? productoUpdate.Stock - cantidad
                        : productoUpdate.Stock + cantidad;

                    // Verificación de stock insuficiente en caso de "VENTA"
                    if (nuevoStock < 0)
                    {
                        throw new Exception("Stock insuficiente");
                    }

                    // Actualización del stock del producto
                    productoUpdate.Stock = nuevoStock;
                    await _productoRepository.UpdateProductoCompra(productoUpdate);

                    // Creación del detalle del comprobante
                    DetalleComprobante detalle = new DetalleComprobante
                    {
                        Cantidad = cantidad,
                        producto = productoUpdate,
                        comprobante = comprobante,
                        PrecioUnitario = decimal.Parse(item.PrecioUnitario.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
                    };

                    // Guardar el detalle del comprobante
                    await _context.DetalleComprobantes.AddAsync(detalle);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> Update(Comprobante comprobante)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ComprobanteDto>> GetAllDevoluciones(int page, int size, string search)
        {
            // Realiza el join entre Comprobante y Usuario usando IdUsuarioCliente
            var query = from comprobante in _context.Comprobantes
                        join usuario in _context.Usuarios
                        on comprobante.IdUsuarioCliente equals usuario.IdUsuario
                        where comprobante.TipoTransaccion.Equals("DEVOLUCION") // Filtra por tipo de transacción
                        select new
                        {
                            Comprobante = comprobante,
                            Usuario = usuario
                        };

            // Filtra los resultados si hay una búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Comprobante.NumeroComprobante.Contains(search) ||
                                         x.Usuario.Identificacion.Contains(search));
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

            // Mapear los elementos a ComprobanteDto
            var comprobanteDto = items.Select(x => new ComprobanteDto
            {
                IdComprobante = x.Comprobante.IdComprobante.ToString(),
                NumeroComprobante = x.Comprobante.NumeroComprobante,
                FechaEmision = x.Comprobante.FechaEmision.ToString("yyyy-MM-dd"), // Formato de fecha
                Total = x.Comprobante.Total.ToString("F2"),
                Iva = x.Comprobante.Iva.ToString("F2"),
                UsuarioClienteNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos // O cualquier otra propiedad relevante
            }).ToList();

            // Retornar el resultado paginado
            return new PagedResult<ComprobanteDto>
            {
                TotalCount = totalCount,
                Items = comprobanteDto
            };
        }

        public async Task<List<ComprobanteResumenDto>> GetResumenVentasPorTipoComprobante(string search)
        {
            // Obtén la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Determina la fecha de inicio del mes actual (primer día del mes)
            DateTime inicioDelMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);

            // Consulta usando LINQ
            var resultados = await _context.Comprobantes
                .Where(c => c.TipoTransaccion == search && c.FechaEmision >= inicioDelMes && c.FechaEmision <= fechaActual) // Filtrar por mes y tipo de transacción
                .GroupBy(c => c.TipoComprobante)
                .Select(g => new ComprobanteResumenDto
                {
                    TipoComprobante = g.Key,
                    Total = Math.Round(g.Sum(c => c.Total), 2)
                })
                .ToListAsync();

            return resultados;
        }


        public async Task<List<ComprobanteResumenDto>> GetResumenVentas()
        {
            // Obtén la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Determina la fecha de inicio del mes actual (primer día del mes)
            DateTime inicioDelMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);

            // Consulta usando LINQ
            var resultados = await _context.Comprobantes
                .Where(c => c.FechaEmision >= inicioDelMes && c.FechaEmision <= fechaActual)
                .GroupBy(c => c.TipoComprobante) // Agrupamos por TipoComprobante
                .Select(g => new ComprobanteResumenDto
                {
                    TipoComprobante = g.Key,
                    Total = Math.Round(
                        g.Where(c => c.TipoTransaccion == "VENTA").Sum(c => c.Total) - // Total de Ventas
                        g.Where(c => c.TipoTransaccion == "DEVOLUCION").Sum(c => c.Total), // Total de Devoluciones
                        2)
                })
                .ToListAsync();

            return resultados;
        }


        public async Task<List<ComprobanteResumenReporteDto>> ObtenerResumenComprobantes(DateTime fechaInicio, DateTime fechaFin, string tipoTransaccion)
        {
            // Consulta base para obtener la información necesaria
            var resumenRaw = await _context.Comprobantes
                .Where(c => c.FechaEmision >= fechaInicio && c.FechaEmision <= fechaFin && c.TipoTransaccion == tipoTransaccion)
                .Join(
                    _context.DetalleComprobantes.Include(d => d.producto), // Incluye explícitamente la relación con Producto
                    c => c.IdComprobante,
                    d => d.IdComprobante,
                    (c, d) => new { Comprobante = c, Detalle = d }
                )
                .Join(
                    _context.Usuarios,
                    cd => cd.Comprobante.IdUsuarioCliente,
                    u => u.IdUsuario,
                    (cd, u) => new { cd.Comprobante, cd.Detalle, Usuario = u }
                )
                .GroupBy(g => new
                {
                    g.Comprobante.NumeroComprobante,
                    g.Usuario.Nombres,
                    g.Usuario.Apellidos,
                    g.Comprobante.FechaEmision,
                    g.Comprobante.TipoPago
                })
                .Select(group => new
                {
                    NumeroComprobante = group.Key.NumeroComprobante,
                    Nombres = group.Key.Nombres,
                    Apellidos = group.Key.Apellidos,
                    TipoPago = group.Key.TipoPago,
                    FechaEmision = group.Key.FechaEmision,
                    Subtotal = group.Sum(x => x.Comprobante.Subtotal),
                    Total = group.Sum(x => x.Comprobante.Total),
                    Items = group.Count(),
                    Detalles = group.Select(x => new
                    {
                        x.Detalle.Cantidad, // Cantidad del producto
                        ProductoNombre = x.Detalle.producto.NombreProducto, // Nombre del producto
                        ProductoDescripcion = x.Detalle.producto.Descripcion // Descripción del producto
                    }).ToList()
                })
                .ToListAsync();

            // Proyección final para el DTO
            var resumen = resumenRaw.Select(raw => new ComprobanteResumenReporteDto
            {
                NumeroComprobante = raw.NumeroComprobante,
                Nombres = raw.Nombres,
                Apellidos = raw.Apellidos,
                TipoPago = raw.TipoPago,
                FechaEmision = raw.FechaEmision,
                Subtotal = raw.Subtotal,
                Total = raw.Total,
                Items = raw.Items,
                NombresProductos = raw.Detalles
                    .Where(d => !string.IsNullOrEmpty(d.ProductoNombre)) // Filtra solo los productos válidos
                    .Select(d => $"{d.ProductoNombre} (Descripción: {d.ProductoDescripcion}, Cantidad: {d.Cantidad})") // Incluye descripción y cantidad
                    .Distinct() // Evita duplicados
                    .ToList()
            }).ToList();

            return resumen;
        }

        public async Task<List<ComprobanteResumenReporteDto>> ObtenerReporteCompras(DateTime fechaInicio, DateTime fechaFin)
        {
            var resumen = _context.Compras
    .Where(c => c.FechaCompra >= fechaInicio && c.FechaCompra <= fechaFin)
    .Join(
        _context.DetalleCompras.Include(d => d.producto), // Incluye la relación con Producto
        c => c.IdCompra,
        d => d.IdCompra,
        (c, d) => new { Compra = c, Detalle = d }
    )
    .Join(
        _context.Usuarios,
        cd => cd.Compra.IdUsuarioProveedor,
        u => u.IdUsuario,
        (cd, u) => new { cd.Compra, cd.Detalle, Usuario = u }
    )
    .GroupBy(x => new
    {
        x.Compra.NumeroCompra,
        x.Usuario.Nombres,
        x.Usuario.Apellidos,
        x.Compra.FechaCompra,
        x.Compra.TotalCompra
    })
    .Select(g => new ComprobanteResumenReporteDto
    {
        NumeroComprobante = g.Key.NumeroCompra,
        Nombres = g.Key.Nombres,
        Apellidos = g.Key.Apellidos,
        FechaEmision = g.Key.FechaCompra,
        Total = g.Key.TotalCompra,
        Items = g.Count(),
        DetallesProductos = g.Select(x => new DetalleProductoDto
        {
            NombreProducto = x.Detalle.producto != null ? x.Detalle.producto.NombreProducto : "Sin producto",
            Cantidad = x.Detalle.CantidadInicial.ToString(),
            Descripcion = x.Detalle.producto != null ? x.Detalle.producto.Descripcion : "Sin descripción"
        }).ToList() // Crea una lista de productos por cada grupo
    })
    .ToList();

            return resumen;
        }

        public async Task<int> GetUltimoNumeroComprobante(string tipoComprobante)
        {
            int numeroComprobante = 0;
            var ultimoNumeroComprobante = await _context.Comprobantes
                .Where(c => c.TipoComprobante == tipoComprobante)
                .OrderByDescending(c => c.NumeroComprobante)
                .Select(c => c.NumeroComprobante)
                .FirstOrDefaultAsync();

            if (ultimoNumeroComprobante != null ) {
                numeroComprobante = Int32.Parse(ultimoNumeroComprobante) + 1;
            } else {
                numeroComprobante++;
            }

            return numeroComprobante;
        }

        private string GuardarArchivoPdf(string fileBase64)
        {
            try
            {
                var base64Data = fileBase64.Split(",")[1]; // Remover encabezado Base64
                var fileBytes = Convert.FromBase64String(base64Data);

                // Crear la carpeta si no existe
                if (!Directory.Exists(_uploadFolderPath))
                {
                    Directory.CreateDirectory(_uploadFolderPath);
                }

                var filePath = Path.Combine(_uploadFolderPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(filePath, fileBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo PDF: {ex.Message}", ex);
            }
        }

        public async Task<ComprobanteDto> GetComprobanteVentaDevolucion(string identificacion, string numeroComprobante, string tipoTransaccion, string tipoComprobante)
        {

            //// Validación previa: Verificar si ya existe un comprobante con los mismos datos de entrada
            //var comprobanteExistente = await _context.Comprobantes
            //    .FirstOrDefaultAsync(c =>
            //        c.usuarioCliente.Identificacion == identificacion &&
            //        c.NumeroComprobante == numeroComprobante &&
            //        c.TipoTransaccion == "DEVOLUCION");

            //if (comprobanteExistente != null)
            //{
            //    // Si ya existe un comprobante, no continuar y retornar null o alguna otra respuesta
            //    throw new Exception("Devolucion ya registrada con estos datos"); // O puedes lanzar una excepción o retornar un mensaje de error
            //}

            var comprobante = await _context.Comprobantes
         .Include(c => c.detallesComprobante)
         .Include(c => c.usuarioCliente)
         .Include(c => c.usuarioComerciante)
         .FirstOrDefaultAsync(c =>
             c.usuarioCliente.Identificacion == identificacion &&
             c.NumeroComprobante == numeroComprobante &&
             c.TipoTransaccion == tipoTransaccion &&
             c.TipoComprobante == tipoComprobante);

            if (comprobante == null)
            {
                return null;
            }

            // Leer y convertir el archivo PDF a Base64
            string? fileBase64 = null;
            if (!string.IsNullOrEmpty(comprobante.PathPdf) && File.Exists(comprobante.PathPdf))
            {
                var fileBytes = await File.ReadAllBytesAsync(comprobante.PathPdf);
                fileBase64 = Convert.ToBase64String(fileBytes);
            }

            var comprobanteDto = new ComprobanteDto
            {
                IdComprobante = comprobante.IdComprobante.ToString(),
                NumeroComprobante = comprobante.NumeroComprobante,
                TipoComprobante = comprobante.TipoComprobante,
                TipoTransaccion = comprobante.TipoTransaccion,
                TipoPago = comprobante.TipoPago,
                FechaEmision = comprobante.FechaEmision.ToString("yyyy-MM-dd"), // Formato de fecha
                Subtotal = comprobante.Subtotal.ToString("F2"), // Formato decimal con dos decimales
                Total = comprobante.Total.ToString("F2"),
                Iva = comprobante.Iva.ToString(),
                IdUsuarioCliente = comprobante.IdUsuarioCliente?.ToString(),
                IdUsuarioComerciante = comprobante.IdUsuarioComerciante?.ToString(),
                NombreCliente = comprobante.usuarioCliente?.Nombres + " " + comprobante.usuarioCliente?.Apellidos,
                Identificacion = comprobante.usuarioCliente?.Identificacion?.ToString(),
                Direccion = comprobante.usuarioCliente?.Direccion?.ToString(),
                UsuarioClienteNombre = comprobante.usuarioCliente?.Nombres + " " + comprobante.usuarioCliente?.Apellidos,
                FileBase64 = fileBase64
            };

            return comprobanteDto;
        }
    }
}
