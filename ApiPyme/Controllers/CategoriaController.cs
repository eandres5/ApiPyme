using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        // GET: api/Categoria/getLista
        [HttpGet("getLista")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllCategorias()
        {
            var categorias = await _categoriaRepository.GetAllCategorias();
            return Ok(categorias);
        }

        // GET: api/Categoria/{id}
        [HttpGet("getCategoria/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _categoriaRepository.GetCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        // POST: api/Categoria/save
        [HttpPost("save")]
        public async Task<ActionResult> CreateCategoria(Categoria categoria)
        {
            var result = await _categoriaRepository.SaveCategoria(categoria);
            if (result)
            {
                return CreatedAtAction(nameof(GetCategoria), new { id = categoria.IdCategoria }, categoria);
            }
            return BadRequest("Error creating category");
        }

        // PUT: api/Categoria/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCategoria(int id, Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest("Category ID mismatch");
            }

            var result = await _categoriaRepository.UpdateCategoria(categoria);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }

        // DELETE: api/Categoria/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCategoria(int id)
        {
            var result = await _categoriaRepository.DeleteCategoria(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}
