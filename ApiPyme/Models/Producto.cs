using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class Producto: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_producto")]
        public int IdProducto { get; set; }
        [Required]
        [Column("nombre_producto")]
        public string NombreProducto { get; set; }
        [Column("descripcion")]
        public string? Descripcion { get; set; }
        [Required]
        [Column("stock")]
        public int Stock { get; set; }
        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }
        [Column("nombre_categoria")]
        public string? NombreCategoria { get; set; }
        [Column("id_usuario_proveedor")]
        public int? IdUsuarioProveedor { get; set; }
        public Usuario? usuarioProveedor { get; set; }
        [Column("qr_path")]
        public string? QrPath { get; set; }
        [Column("estado_producto")]
        public string? EstadoProducto { get; set; }
        public ICollection<DetalleCompra>? detallesCompra { get; set; }
        public ICollection<DetalleComprobante>? detallesComprobante { get; set; }
    }
}
