namespace ERP.Api.Models;

public class TipoDescuento
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public ICollection<Empleado> Empleados { get; set; } = [];
}
