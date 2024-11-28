using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class DetallePedido : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_detalle_pedido")]
        public int IdDetallePedido { get; set; }
        [Required]
        [Column("estado_pedido")]
        public string? EstadoPedido { get; set; }
        [Column("id_producto")]
        public int? ProductoId { get; set; }
        [Column("id_pedido")]
        public int? PedidoId { get; set; }
        public Producto? producto { get; set; }
        public Pedido? pedido { get; set; }

    }
}
