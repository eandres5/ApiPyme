using ApiPyme.Dto;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleComprobanteController : ControllerBase
    {
        private readonly IDetalleComprobanteRepository _detalleComprobanteRepository;
        public DetalleComprobanteController(IDetalleComprobanteRepository detalleComprobanteRepository)
        {
            _detalleComprobanteRepository = detalleComprobanteRepository;

        }

        [HttpPost("getDetalleProducto")]
        public async Task<ActionResult<IEnumerable<DetalleProductoDto>>> GetDetalleProducto(DetalleProductoDto detalleProductoDto)
        {
            var comprobantes = await _detalleComprobanteRepository.GetDetalleProductoDto(Int32.Parse(detalleProductoDto.IdComprobante));
            return Ok(comprobantes);
        }
    }
}
