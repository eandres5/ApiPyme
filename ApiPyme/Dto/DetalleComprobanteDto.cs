using ApiPyme.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPyme.Dto
{
    public class DetalleComprobanteDto
    {
        public string? IdDetalleComprobante { get; set; }
        public string PrecioUnitario { get; set; }
        public string Cantidad { get; set; }
        public string IdProducto { get; set; }
    }
}
