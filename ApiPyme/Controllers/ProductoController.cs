using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System.Text.Json;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        // GET: api/Producto/getLista
        [HttpGet("getLista")]
        public async Task<ActionResult<IEnumerable<Producto>>> getAllProductos()
        {
            var producto = await _productoRepository.GetAllProductos();
            return Ok(producto);
        }

        // POST: api/Producto/save
        [HttpPost("save")]
        public async Task<ActionResult> CreateProducto(ProductoDto productoDto)
        {
            var result = await _productoRepository.SaveProducto(productoDto);
            if (result)
            {
                return Ok(new { message = "Registro Creado" });
            }
            return BadRequest(new { message = "Error al crear" });
        }

        // GET: api/Producto/{id}
        [HttpGet("getProducto/{id}")]
        public async Task<ActionResult<Producto>> getProducto(string id)
        {
            var producto = await _productoRepository.GetProducto(Int32.Parse(id));
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        // GET: api/Producto/getProductoUsuario
        [HttpGet("getProductoUsuario/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByUsuario(int page, int size, string search)
        {
            if (search.Equals("null")) { 
                search = null;
            }
            var productos = await _productoRepository.GetProductosByUsuario(page, size, search);
            return Ok(productos);
        }

        [HttpGet("getProductoUsuarioDescripcion/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByUsuarioDescripcion(int page, int size, string search)
        {
            if (search.Equals("null"))
            {
                search = null;
            }
            var productos = await _productoRepository.GetProductosByUsuarioDescripcion(page, size, search);
            return Ok(productos);
        }

        [HttpGet("getProductoProveedor/{page}/{size}/{identificacion}/{search}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductoProveedor(int page, int size, string identificacion, [FromQuery] string search = null)
        {
            var productos = await _productoRepository.GetProductosByProveedor(page, size, identificacion, search);
            return Ok(productos);
        }


        [HttpGet("getProductoClientes/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductoCliente(int page, int size, [FromQuery] string search = null)
        {
            var productos = await _productoRepository.GetProductosCliente(page, size, search);
            return Ok(productos);
        }

        [HttpGet("getProductoQr/{idProducto}")]
        public async Task<ActionResult<ProductoDto>> GetProductoConQr(int idProducto)
        {
            var producto = await _productoRepository.GetProductoConQr(idProducto);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPut("updateProducto")]
        public async Task<ActionResult> UpdateProducto(ProductoDto productoDto)
        {
            try
            {
                Producto producto = new Producto();
                var result = await _productoRepository.UpdateProductoDto(productoDto);
                return Ok(new { message = "Registro Actualizado" });
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest(new { message = $"Error: {ex.Message}" });

            }
        }

        [HttpPost("deleteProducto")]
        public async Task<ActionResult> DeleteProducto(ProductoDto productoDto)
        {
            try
            {
                var usuarios = await _productoRepository.DeleteProducto(Int32.Parse(productoDto.IdProducto));
                return Ok(new { message = "Registro Eliminado" });
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest(new { message = $"Error: {ex.Message}" });

            }
        }


        [HttpGet("getBajoStock")]
        public async Task<ActionResult<ProductoDto>> ObtenerBajoStock()
        {
            var producto = await _productoRepository.ObtenerBajoStock();
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

    }
}
