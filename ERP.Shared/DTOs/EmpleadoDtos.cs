namespace ERP.Shared.DTOs;

public record EmpleadoDto(
    int Id,
    string Nombre,
    string Apellido,
    string Dni,
    string Cargo,
    decimal SalarioBase,
    string TipoDescuento,
    bool Activo);

public record EmpleadoCreateDto(
    string Nombre,
    string Apellido,
    string Dni,
    string Cargo,
    decimal SalarioBase,
    int IdTipoDescuento);

public record EmpleadoUpdateDto(
    string Nombre,
    string Apellido,
    string Cargo,
    decimal SalarioBase,
    int IdTipoDescuento);
