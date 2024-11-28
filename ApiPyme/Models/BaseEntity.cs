using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class BaseEntity
    {
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime? UpdateAt { get; set; }
        [Column("observacion")]
        public string? Observacion { get; set; }
        [Column("usuario_creacion")]
        public string? UsuarioCreacion { get; set; }
        [Column("usuario_modificacion")]
        public string? UsuarioModificacion { get; set; }
        [Column("activo")]
        public bool? Activo { get; set; } = true;
    }
}
