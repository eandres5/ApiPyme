using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class Comprobante: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_comprobante")]
        public int IdComprobante { get; set; }
        [Required]
        [StringLength(50)]
        [Column("numero_comprobante")]
        public string NumeroComprobante { get; set; }
        [Required]
        [Column("tipo_comprobante")]
        public string TipoComprobante { get; set; }
        [Required]
        [Column("tipo_transaccion")]
        public string TipoTransaccion { get; set; }
        [Required]
        [Column("tipo_pago")]
        public string TipoPago { get; set; }
        [Required]
        [Column("fecha_emision")]
        public DateTime FechaEmision { get; set; }
        [Column("iva")]
        public int Iva { get; set; }
        [Column("subtotal")]
        public Decimal Subtotal { get; set; }
        [Required]
        [Column("total")]
        public Decimal Total { get; set; }
        [Column("id_usuario_cliente")]
        public int? IdUsuarioCliente { get; set; }
        [Column("id_usuario_comerciante")]
        public int? IdUsuarioComerciante { get; set; }
        [Column("path_pdf")]
        public string? PathPdf { get; set; }
        public Usuario? usuarioCliente { get; set; }
        public Usuario? usuarioComerciante { get; set; }
        public ICollection<DetalleComprobante>? detallesComprobante { get; set; }
    }
}
