namespace ERP.Api.Models;

public class Empleado
{
    public int IdEmpleado { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public decimal SalarioBase { get; set; }
    public int? IdTipoDescuento { get; set; }
    public TipoDescuento? TipoDescuento { get; set; }
    public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool Activo { get; set; } = true;
    public ICollection<Planilla> Planillas { get; set; } = [];
}
