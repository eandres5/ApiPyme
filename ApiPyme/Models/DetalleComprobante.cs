using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class DetalleComprobante: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_detalle_comprobante")]
        public int IdDetalleComprobante { get; set; }
        [Column("precio_unitario")]
        public Decimal PrecioUnitario { get; set; }
        [Column("cantidad")]
        public int Cantidad { get; set; }
        [Column("id_producto")]
        public int? IdProducto { get; set; }
        public Producto? producto { get; set; }
        [Column("id_comprobante")]
        public int? IdComprobante { get; set; }
        public Comprobante? comprobante { get; set; }
    }
}
