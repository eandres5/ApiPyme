using ApiPyme.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Dto
{
    public class UsuarioDto : BaseDto
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identificacion { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? IdRol { get; set; }
        public string? Password { get; set; }
    }
}
