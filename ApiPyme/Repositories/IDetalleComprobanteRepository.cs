using ApiPyme.Dto;

namespace ApiPyme.Repositories
{
    public interface IDetalleComprobanteRepository
    {
        Task<List<DetalleProductoDto>> GetDetalleProductoDto(int idComprobante);
    }
}
