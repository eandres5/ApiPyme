using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class DetalleCompra: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_detalle_compra")]
        public int IdDetalleCompra { get; set; }
        [Column("cantidad_inicial")]
        public int CantidadInicial { get; set; }
        [Column("id_producto")]
        public int? IdProducto { get; set; }
        public Producto? producto { get; set; }
        [Column("id_compra")]
        public int? IdCompra { get; set; }
        public Compra? compra { get; set; }
        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }
    }
}
