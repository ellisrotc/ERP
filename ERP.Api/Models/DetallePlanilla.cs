namespace ERP.Api.Models;

public class DetallePlanilla
{
    public int IdDetalle { get; set; }
    public int IdPlanilla { get; set; }
    public Planilla Planilla { get; set; } = null!;
    public decimal Afp { get; set; }
    public decimal Onp { get; set; }
    public decimal Cts { get; set; }
    public decimal Gratificacion { get; set; }
    public decimal HorasExtras { get; set; }
}
