namespace ERP.Api.Models;

public class Comprobante
{
    public int IdComprobante { get; set; }
    public int? IdTipoComprobante { get; set; }
    public TipoComprobante? TipoComprobante { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string? Serie { get; set; }
    public string Ruc { get; set; } = string.Empty;
    public string? RazonSocial { get; set; }
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal Monto { get; set; }
    public decimal Igv { get; set; }
    public bool Activo { get; set; } = true;
}
