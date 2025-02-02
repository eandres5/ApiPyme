using ApiPyme.Dto;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetComprobante(int id, string tipoTransaccion)
        {
            var comprobantes = await _comprobanteRepository.GetComprobante(id, tipoTransaccion);
            return Ok(comprobantes);
        }

        [HttpGet("getComprobanteVentaDevolucion/{identificacion}/{numeroComprobante}/{tipoTransaccion}/{tipoComprobante}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetComprobante(string identificacion, string numeroComprobante, string tipoTransaccion, string tipoComprobante)
        {
            try
            {
                var comprobantes = await _comprobanteRepository.GetComprobanteVentaDevolucion(identificacion, numeroComprobante, tipoTransaccion, tipoComprobante);
                return Ok(comprobantes);
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje detallado del error
                return BadRequest(new { message = $"{ex.Message}" });
            }

        }


        [HttpGet("getReporteComprobantes/{fechaInicio}/{fechaFin}/{transaccion}")]
        public async Task<ActionResult<IEnumerable<ComprobanteResumenReporteDto>>> ObtenerResumenComprobantes(string fechaInicio, string fechaFin, string transaccion)
        {

            try
            {
                DateTime fechaUno = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaDos = DateTime.ParseExact(fechaFin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var comprobantes = await _comprobanteRepository.ObtenerResumenComprobantes(fechaUno, fechaDos, transaccion);
                return Ok(comprobantes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }

        [HttpGet("getReporteCompras/{fechaInicio}/{fechaFin}")]
        public async Task<ActionResult<IEnumerable<ComprobanteResumenReporteDto>>> ObtenerReporteCompras(string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime fechaUno = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaDos = DateTime.ParseExact(fechaFin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var comprobantes = await _comprobanteRepository.ObtenerReporteCompras(fechaUno, fechaDos);
                return Ok(comprobantes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }
        
        [HttpGet("getResumenVentas")]
        public async Task<ActionResult<IEnumerable<ComprobanteResumenReporteDto>>> GetResumenVentas()
        {

            try
            {
                var comprobantes = await _comprobanteRepository.GetResumenVentas();
                return Ok(comprobantes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }

        [HttpGet("getUltimoComprobante/{tipoComprobante}")]
        public async Task<ActionResult<IEnumerable<ComprobanteResumenReporteDto>>> GetUltimoNumeroComprobante(string tipoComprobante)
        {

            try
            {
                var comprobantes = await _comprobanteRepository.GetUltimoNumeroComprobante(tipoComprobante);
                return Ok(comprobantes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }

        [HttpGet("getReporteGrafico/{transaccion}")]
        public async Task<ActionResult<IEnumerable<ComprobanteResumenReporteDto>>> ObtenerReporteGrafico(string transaccion)
        {
            try
            {
                var comprobantes = await _comprobanteRepository.ObtenerReporteGrafico(transaccion);
                return Ok(comprobantes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }

        }

    }

}
