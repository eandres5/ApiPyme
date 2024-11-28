using ApiPyme.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPyme.Dto
{
    public class ProductoDto: BaseDto
    {
        public string? IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public CategoriaDto? categoriaDto { get; set; }
        public string Stock { get; set; }
        public string Precio { get; set; }
        public string IdProveedor { get; set; }
        public string QrCodeImage { get; set; }
        public string Proveedor { get; set; }
    }
}
