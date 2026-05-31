namespace ERP.Shared.DTOs;

public record ComprobanteCreateDto(
    int IdTipo,
    string Numero,
    string Serie,
    string Ruc,
    string RazonSocial,
    DateTime Fecha,
    decimal Monto);

public record ComprobanteDto(
    int Id,
    string TipoNombre,
    string Numero,
    string Serie,
    string Ruc,
    string RazonSocial,
    DateTime Fecha,
    decimal Monto,
    decimal Igv,
    bool Activo);
