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
        public int Items { get; set; }
    }
}
