using ApiPyme.Dto;
using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface ICompraRepository
    {
        Task<CompraDto> GetCompra(int id);
        Task<bool> Save(CompraDto compraDto);
        Task<bool> Update(Compra compra);
        Task<bool> Delete(int id);
        Task<PagedResult<CompraDto>> getAllCompras(int page, int size, string search);
        Task<List<ComprobanteResumenDto>> GetResumenCompras();
    }
}
