using ApiPyme.Dto;
using ApiPyme.Models;
using ApiPyme.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiPyme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly IUsuarioRepository _usuarioRepository;
        public LoginController(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        // POST: api/login/login
        [HttpPost("login")]
        public async Task<ActionResult> login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Identificacion) || string.IsNullOrEmpty(loginRequest.Password)) {
                return BadRequest("Credenciales no coinciden");
            }
            var usuario = await _usuarioRepository.GetLogin(loginRequest);
            if (usuario == null) { 
                return Unauthorized("Credenciales no validas");
            }

            // si devuele el usuario se crea el token para devolver
            var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
            //var claims = new ClaimsIdentity();
            //claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Identificacion));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Identificacion));
            claims.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombres + " " + usuario.Apellidos));

            // Agregar roles del usuario a los claims
            foreach (var role in usuario.UsuarioRoles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role.rol.Nombre));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(60), // aqui se agrega el tiempo del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenGenerated = tokenHandler.WriteToken(tokenConfig);

            return StatusCode(StatusCodes.Status200OK, new { token = tokenGenerated });
        }
    }
}
