namespace ERP.Api.Models;

public class TipoComprobante
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? SeriePrefix { get; set; }
    public ICollection<Comprobante> Comprobantes { get; set; } = [];
}
