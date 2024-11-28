using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class Rol: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_rol")]
        public int IdRol { get; set; }
        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }
        public ICollection<UsuarioRol>? UsuarioRoles { get; set; }
    }
}
