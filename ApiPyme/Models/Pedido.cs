using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class Pedido: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_pedido")]
        public int IdPedido { get; set; }
        [Required]
        [StringLength(50)]
        [Column("numero_pedido")]
        public string NumeroPedido { get; set; }
        [Column("id_usuario_proveedor")]
        public int? IdUsuarioProveedor { get; set; }
        [Column("id_usuario_comerciante")]
        public int? IdUsuarioComerciante { get; set; }
        public Usuario? usuarioProveedor { get; set; }
        public Usuario? usuarioComerciante { get; set; }
        public ICollection<DetallePedido>? DetalleProductos { get; set; }

    }
}
