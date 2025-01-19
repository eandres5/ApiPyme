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
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // GET: api/Usuario/getLista
        [HttpGet("getLista/{page}/{size}/{search}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> getAllUsuarios(int page, int size, [FromQuery] string search = null)
        {
            var usuarios = await _usuarioRepository.GetAllUsuarios(page, size, search);
            return Ok(usuarios);
        }

        [HttpGet("getProveedores/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> getUsuariosProveedores(int page, int size, string search)
        {
            try
            {
                if (search.Equals("null")) { 
                    search = null;
                }

                var usuarios = await _usuarioRepository.GetAllUsuariosProveedor(page, size, search);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest($"Error al crear la Compra: {ex.Message}");
            }
        }

        [HttpGet("getClientes/{page}/{size}/{search}")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAllUsuariosClientes(int page, int size, string search)
        {
            try
            {
                if (search.Equals("null"))
                {
                    search = null;
                }

                var usuarios = await _usuarioRepository.GetAllUsuariosClientes(page, size, search);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest($"Error al crear la Compra: {ex.Message}");
            }
        }


        // POST: api/Usuario/save
        [HttpPost("save")]
        public async Task<ActionResult> CreateUsuario(UsuarioDto UsuarioDto)
        {
            try
            {
                var result = await _usuarioRepository.SaveUsuario(UsuarioDto);

                if(result)
                {
                    return Ok(new { message = "Usuario registrado" });
                }
                return BadRequest(new { message = "Error al crear registro" });

            }
            catch (Exception ex)
            {
                // Devuelve un mensaje detallado del error
                return BadRequest(new { message = $"Error al crear: {ex.Message}" });
            }
        }

        // GET: api/Usuario/{id}
        [HttpGet("getUsuario/{id}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> getUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetUsuario(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPut("updateProveedor")]
        public async Task<ActionResult> UpdateUsuarioProveedor(UsuarioDto usuario)
        {

            try
            {
                var result = await _usuarioRepository.UpdateUsuario(usuario);
                return Ok(new { message = "Registro Actualizado" });
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest(new { message = $"Error: {ex.Message}" });

            }
        }

        [HttpGet("getUsuarioProveedor/{identificacion}")]
        public async Task<ActionResult<UsuarioDto>> getUsuarioProveedor(string identificacion)
        {
            try
            {
                var usuarios = await _usuarioRepository.GetUsuarioByIdentificaion(identificacion);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest($"Error al encontrar usuario: {ex.Message}");
            }
        }

        [HttpPost("deleteUsuario")]
        public async Task<ActionResult<UsuarioDto>> DeleteUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                var usuarios = await _usuarioRepository.DeleteUsuario(usuarioDto);
                return Ok(new { message = "Registro Eliminado" });
            }
            catch (Exception ex)
            {
                // Devuelve un mensaje de error
                return BadRequest(new { message = $"Error: {ex.Message}" });

            }
        }

    }
}
