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
        public ComprobanteRepositoryImpl(AppDbContext context, IProductoRepository productoRepository, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
            
        }

        public async Task<PagedResult<ComprobanteDto>> GetAllVentas(int page, int size, string search)
        {
            // Asegura que los valores de paginación sean válidos
            page = Math.Max(page, 1);  // Asegura que la página mínima sea 1
            size = Math.Max(size, 10); // Establecer un tamaño predeterminado si no es válido

            // Calcula el número de elementos a saltar
            int skip = (page - 1) * size;

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
                UsuarioClienteNombre = comprobante.usuarioCliente?.Nombres + " " + comprobante.usuarioCliente?.Apellidos, // Asumiendo que Usuario tiene un campo 'Nombre'
                DetalleComprobantes = comprobante.detallesComprobante?.Select(d => new DetalleComprobanteDto
                {
                    IdDetalleComprobante = d.IdDetalleComprobante.ToString(),
                    PrecioUnitario = d.PrecioUnitario.ToString("F2"), // Formato decimal con dos decimales
                    Cantidad = d.Cantidad.ToString(),
                    IdProducto = d.IdProducto?.ToString()
                }).ToList()
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
                Comprobante comprobante = new Comprobante();
                comprobante.NumeroComprobante = comprobanteDto.NumeroComprobante;
                comprobante.TipoComprobante = comprobanteDto.TipoComprobante;
                comprobante.usuarioCliente = usuarioCliente;
                comprobante.usuarioComerciante = usuarioComerciante;
                comprobante.TipoTransaccion = comprobanteDto.TipoTransaccion;
                comprobante.FechaEmision = DateTime.ParseExact(comprobanteDto.FechaEmision, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                comprobante.Iva = Int32.Parse(comprobanteDto.Iva);
                comprobante.Subtotal = decimal.Parse(comprobanteDto.Subtotal, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                comprobante.Total = decimal.Parse(comprobanteDto.Total, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);


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
                    Total = g.Sum(c => c.Total)
                })
                .ToListAsync();

            return resultados;
        }

    }
}
