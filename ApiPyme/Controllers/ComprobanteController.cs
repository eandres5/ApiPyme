using ApiPyme.Dto;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobanteController : ControllerBase
    {
        private readonly IComprobanteRepository _comprobanteRepository;
        public ComprobanteController(IComprobanteRepository comprobanteRepository)
        {
            _comprobanteRepository = comprobanteRepository;
            
        }

        // GET: api/Comprobante/getListaVentas
        [HttpGet("getListaVentas/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetAllVentas(int page, int size, [FromQuery] string search = null)
        {
            var ventas = await _comprobanteRepository.GetAllVentas(page, size, search);
            return Ok(ventas);
        }

            // GET: api/Usuario/getLista
        [HttpGet("getLista/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> getAllCompras(int page, int size, [FromQuery] string search = null)
        {
            var comprobantes = await _comprobanteRepository.GetAllVentas(page, size, search);
            return Ok(comprobantes);
        }

        [HttpGet("getListaDevolucion/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetAllDevolucion(int page, int size, [FromQuery] string search = null)
        {
            var comprobantes = await _comprobanteRepository.GetAllDevoluciones(page, size, search);
            return Ok(comprobantes);
        }


        // POST: api/Comprobante/save
        [HttpPost("save")]
        public async Task<ActionResult> CreateComprobante(ComprobanteDto comprobanteDto)
        {
            try
            {
                var result = await _comprobanteRepository.Save(comprobanteDto);
                if (result)
                {
                    return Ok(new { message = "Comprobante Creado" });
                }
                return BadRequest(new { message = "Error al crear comprobante" });
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje detallado del error
                return BadRequest(new { message = $"Error al crear la comprobante: {ex.Message}" });
            }
        }

        [HttpGet("getResumenComprobantes/{tipoTransaccion}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetResumenVentasPorTipoComprobante(string tipoTransaccion)
        {
            var comprobantes = await _comprobanteRepository.GetResumenVentasPorTipoComprobante(tipoTransaccion);
            return Ok(comprobantes);
        }


        [HttpGet("getComprobante/{id}/{tipoTransaccion}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetResumenVentasPorTipoComprobante(int id, string tipoTransaccion)
        {
            var comprobantes = await _comprobanteRepository.GetComprobante(id, tipoTransaccion);
            return Ok(comprobantes);
        }

    }

}
