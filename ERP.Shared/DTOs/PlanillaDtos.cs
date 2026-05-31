namespace ERP.Shared.DTOs;

public record PlanillaResultDto(
    int IdPlanilla,
    string NombreEmpleado,
    string Periodo,
    decimal TotalBruto,
    decimal TotalDescuentos,
    decimal TotalNeto,
    decimal Afp,
    decimal Onp,
    decimal Cts,
    decimal Gratificacion);

public record PlanillaCalcularRequest(string Periodo);
