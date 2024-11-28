using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Models
{
    public class Usuario: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        [Required]
        [Column("nombres")]
        public string Nombres { get; set; }
        [Required]
        [Column("apellidos")]
        public string Apellidos { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [Required]
        [Column("identificacion")]
        public string Identificacion { get; set; }
        [Column("telefono")]
        public string? Telefono { get; set; }
        [Column("Direccion")]
        public string? Direccion { get; set; }
        public ICollection<UsuarioRol>? UsuarioRoles { get; set; }
        public ICollection<Producto>? Productos { get; set; }
        public ICollection<Pedido>? PedidosComerciante { get; set; }
        public ICollection<Pedido>? PedidosProveedor { get; set; }
        public ICollection<Compra>? ComprasComerciante { get; set; }
        public ICollection<Compra>? ComprasProveedor { get; set; }
        public ICollection<Comprobante>? ComprobantesComerciante { get; set; }
        public ICollection<Comprobante>? ComprobantesCliente { get; set; }

    }
}
