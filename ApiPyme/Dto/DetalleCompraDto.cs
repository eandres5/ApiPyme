namespace ApiPyme.Dto
{
    public class DetalleCompraDto: BaseDto
    {
        public string? IdDetalleCompra { get; set; }
        public string CantidadInicial { get; set; }
        public string? IdProducto { get; set; }
        public string? IdCompra { get; set; }
        public string PrecioUnitario { get; set; }
        public string? NombreProducto { get; set; }
        public string? Descripcion { get; set; }
        public string? Stock { get; set; }
        public string? Precio { get; set; }
    }
}
