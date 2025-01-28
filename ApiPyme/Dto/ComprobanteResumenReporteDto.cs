namespace ApiPyme.Dto
{
    public class ComprobanteResumenReporteDto
    {
        public string NumeroComprobante { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string TipoPago { get; set; }
        public int Items { get; set; }
        public List<string> NombresProductos { get; set; }
        public List<DetalleProductoDto> DetallesProductos { get; set; }


    }
}
