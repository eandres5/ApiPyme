using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class RecuperacionPassword : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        [Column("token")]
        public String Token { get; set; }
        [Column("fecha_expiracion")]
        public DateTime FechaExpiracion { get; set; }
    }
}
