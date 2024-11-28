using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class Inventario: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_inventario")]
        public int IdInventario { get; set; }
        [Column("id_comerciante")]
        public int? IdUsuarioComerciante { get; set; }
        [Required]
        [Column("stock")]
        public int Stock { get; set; }
        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }
        [Column("id_producto")]
        public int? IdProducto { get; set; }
        public Usuario? usuarioComerciante { get; set; }
        public Producto? producto { get; set; }
    }
}
