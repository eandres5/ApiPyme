﻿namespace ApiPyme.Dto
{
    public class ComprobanteVentaDto
    {
        public string? IdComprobante { get; set; }
        public string? NumeroComprobante { get; set; }
        public string? TipoComprobante { get; set; }
        public string? TipoTransaccion { get; set; }
        public string? FechaEmision { get; set; }
        public string? Subtotal { get; set; }
        public string? Total { get; set; }
        public string? Iva { get; set; }
        public string? IdUsuarioCliente { get; set; }
        public string? IdUsuarioComerciante { get; set; }
        public string? UsuarioClienteNombre { get; set; }
        public string? NombreCliente { get; set; }
        public string? Identificacion { get; set; }
        public string? Direccion { get; set; }
        public List<DetalleComprobanteDto>? DetalleComprobantes { get; set; }
    }
}
