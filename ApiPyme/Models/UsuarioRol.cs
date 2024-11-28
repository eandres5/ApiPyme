using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class UsuarioRol: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario_rol")]
        public int IdUsuarioRol { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int? IdUsuario { get; set; }
        [Column("id_rol")]
        public int? IdRol { get; set; }
        public Usuario? usuario { get; set; }
        public Rol? rol { get; set; }
    }
}
