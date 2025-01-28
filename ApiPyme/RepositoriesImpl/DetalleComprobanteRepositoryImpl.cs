using ApiPyme.Context;
using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiPyme.RepositoriesImpl
{
    public class DetalleComprobanteRepositoryImpl : IDetalleComprobanteRepository
    {
        private readonly AppDbContext _context;
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public DetalleComprobanteRepositoryImpl(AppDbContext context, IProductoRepository productoRepository, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;

        }

        public async Task<List<DetalleProductoDto>> GetDetalleProductoDto(int idComprobante)
        {
            var detalles = await _context.DetalleComprobantes
                .Include(d => d.producto) // Incluye la relación con producto
                .Where(d => d.IdComprobante == idComprobante) // Filtra por el comprobante
                .ToListAsync(); // Obtén todos los registros

            // Proyecta los resultados a DetalleProductoDto
            var detalleProductos = detalles.Select(detalle => new DetalleProductoDto
            {
                IdProducto = detalle.IdProducto.ToString(),
                IdComprobante = detalle.IdComprobante.ToString(),
                NombreProducto = detalle.producto?.NombreProducto,
                Descripcion = detalle.producto?.Descripcion,
                Cantidad = detalle.Cantidad.ToString(),
                Precio = detalle.PrecioUnitario.ToString("F2")
            }).ToList();

            return detalleProductos;
        }
    }
}
