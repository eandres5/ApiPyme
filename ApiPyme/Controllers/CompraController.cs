using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ICompraRepository _compraRepository;

        public CompraController(ICompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }

        // GET: api/Usuario/getLista
        [HttpGet("getLista/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<CompraDto>>> getAllCompras(int page, int size, [FromQuery] string search = null)
        {
            var usuarios = await _compraRepository.getAllCompras(page, size, search);
            return Ok(usuarios);
        }

        // POST: api/Compra/save
        [HttpPost("save")]
        public async Task<ActionResult> CreateCompra(CompraDto compraDto)
        {
            try
            {
                var result = await _compraRepository.Save(compraDto);
                if (result)
                {
                    return Ok("Compra Creada");
                }
                return BadRequest("Error al crear la Compra");
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje detallado del error
                return BadRequest($"Error al crear la Compra: {ex.Message}");
            }
        }

        [HttpGet("getResumenCompras")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetResumenVentasPorTipoComprobante()
        {
            var compras = await _compraRepository.GetResumenCompras();
            return Ok(compras);
        }

        [HttpGet("getCompra/{id}")]
        public async Task<ActionResult<IEnumerable<ComprobanteDto>>> GetCompra(int id)
        {
            var compras = await _compraRepository.GetCompra(id);
            return Ok(compras);
        }

    }
}
