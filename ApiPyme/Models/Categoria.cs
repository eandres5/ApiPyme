using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class Categoria: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }
        [Required]
        [Column("nombre")]
        public string Nombre { get; set; }
        public ICollection<Producto>? productos { get; set; }

    }
}
