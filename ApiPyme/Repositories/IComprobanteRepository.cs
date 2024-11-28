﻿using ApiPyme.Dto;
using ApiPyme.Models;

namespace ApiPyme.Repositories
{
    public interface IComprobanteRepository
    {
        Task<ComprobanteDto> GetComprobante(int id, string tipoTransacion);
        Task<bool> Save(ComprobanteDto comprobanteDto);
        Task<bool> Update(Comprobante comprobante);
        Task<bool> Delete(int id);
        Task<PagedResult<ComprobanteDto>> getAllComprobantes(int page, int size, string search);
        Task<PagedResult<ComprobanteDto>> GetAllVentas(int page, int size, string search);
        Task<PagedResult<ComprobanteDto>> GetAllDevoluciones(int page, int size, string search);
        Task<List<ComprobanteResumenDto>> GetResumenVentasPorTipoComprobante(string search);
    }
}
