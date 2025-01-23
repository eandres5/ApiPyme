using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Models
{
    public class Compra : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_compra")]
        public int IdCompra { get; set; }
        [Required]
        [StringLength(50)]
        [Column("numero_compra")]
        public string NumeroCompra { get; set; }
        [Required]
        [Column("tipo_comprobante")]
        public string TipoComprobante { get; set; }
        [Required]
        [Column("fecha_compra")]
        public DateTime FechaCompra { get; set; }
        [Required]
        [Column("total_compra")]
        public Decimal TotalCompra { get; set; }
        [Column("iva")]
        public int Iva { get; set; }
        [Column("id_usuario_proveedor")]
        public int? IdUsuarioProveedor { get; set; }
        [Column("id_usuario_comerciante")]
        public int? IdUsuarioComerciante { get; set; }
        [Column("path_pdf")]
        public string? PathPdf { get; set; }
        public Usuario? usuarioProveedor { get; set; }
        public Usuario? usuarioComerciante { get; set; }
        public ICollection<DetalleCompra>? detallesCompra { get; set; }
    }
}
