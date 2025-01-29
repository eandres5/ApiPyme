using ApiPyme.Dto;
using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface IComprobanteRepository
    {
        Task<ComprobanteDto> GetComprobante(int id, string tipoTransacion);
        Task<ComprobanteDto> GetComprobanteVentaDevolucion(string identificacion, string numeroComprobante, string tipoTransaccion, string tipoComprobante);
        Task<bool> Save(ComprobanteDto comprobanteDto);
        Task<bool> Update(Comprobante comprobante);
        Task<bool> Delete(int id);
        Task<PagedResult<ComprobanteDto>> getAllComprobantes(int page, int size, string search);
        Task<PagedResult<ComprobanteDto>> GetAllVentas(int page, int size, string search);
        Task<PagedResult<ComprobanteDto>> GetAllDevoluciones(int page, int size, string search);
        Task<List<ComprobanteResumenDto>> GetResumenVentasPorTipoComprobante(string search);
        Task<List<ComprobanteResumenReporteDto>> ObtenerResumenComprobantes(DateTime fechaInicio, DateTime fechaFin, string tipoTransaccion);
        Task<List<ComprobanteResumenReporteDto>> ObtenerReporteCompras(DateTime fechaInicio, DateTime fechaFin);
        Task<List<ComprobanteResumenDto>> GetResumenVentas();
        Task<int> GetUltimoNumeroComprobante(string tipoComprobante);
    }
}
