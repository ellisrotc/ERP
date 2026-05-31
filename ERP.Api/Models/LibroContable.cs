namespace ERP.Api.Models;

public class LibroContable
{
    public int IdLibro { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "PENDIENTE";
    public int? IdUsuario { get; set; }
    public Usuario? Usuario { get; set; }
    public ICollection<ComprobanteLIbro> ComprobantesLibro { get; set; } = [];
}

public class ComprobanteLIbro
{
    public int IdComprobante { get; set; }
    public Comprobante Comprobante { get; set; } = null!;
    public int IdLibro { get; set; }
    public LibroContable Libro { get; set; } = null!;
    public decimal? BaseImponible { get; set; }
}
