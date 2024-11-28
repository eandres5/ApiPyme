namespace ApiPyme.Dto
{
    public class BaseDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        public string? Observacion { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public bool? Activo { get; set; } = true;
    }
}
