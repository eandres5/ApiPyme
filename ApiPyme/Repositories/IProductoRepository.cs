using ApiPyme.Dto;
using ApiPyme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiPyme.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllProductos();
        Task<Producto> GetProducto(int id);
        Task<bool> SaveProducto (ProductoDto productoDto);
        Task<bool> UpdateProducto(Producto producto);
        Task<bool> UpdateProductoDto(ProductoDto productoDto);
        Task<bool> DeleteProducto(int id);
        Task<string> GenerateQR(Producto producto);
        Task<bool> UpdateProductoCompra(Producto producto);
        Task<Producto> SaveProductoCompra(ProductoDto productoDto);
        Task<Producto> GetProductoEstado(int id, string estadoProducto);
        Task<PagedResult<ProductoDto>> GetProductosByUsuario(int page, int size, string search);
        Task<PagedResult<ProductoDto>> GetProductosByProveedor(int page, int size, string identificacion, string search);
        Task<PagedResult<ProductoDto>> GetProductosCliente(int page, int size, string search);
        Task<ActionResult<ProductoDto>> GetProductoConQr(int idProducto);
    }
}
