namespace ERP.Api.Models;

public class Planilla
{
    public int IdPlanilla { get; set; }
    public int IdEmpleado { get; set; }
    public Empleado Empleado { get; set; } = null!;
    public string Periodo { get; set; } = string.Empty;
    public decimal? TotalBruto { get; set; }
    public decimal? TotalDescuentos { get; set; }
    public decimal? TotalNeto { get; set; }
    public DateTime FechaCalculo { get; set; } = DateTime.UtcNow;
    public DetallePlanilla? Detalle { get; set; }
}
