namespace ApiPyme.Dto
{
    public class CompraDto: BaseDto
    {
        public string? IdCompra { get; set; }
        public string? NumeroCompra { get; set; }
        public string? TipoComprobante { get; set; }
        public string? FechaCompra { get; set; }
        public string? TotalCompra { get; set; }
        public string? Iva { get; set; }
        public string? IdUsuarioProveedor { get; set; }
        public string? IdUsuarioComerciante { get; set; }
        public List<DetalleCompraDto>? DetalleCompras { get; set; }
    }
}
