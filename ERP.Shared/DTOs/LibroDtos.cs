namespace ERP.Shared.DTOs;

public record LibroContableDto(
    int IdLibro,
    string Tipo,
    string Periodo,
    DateTime FechaGeneracion,
    string Estado);

public record LibroDetalleDto(
    int IdComprobante,
    string Numero,
    string RazonSocial,
    string Ruc,
    decimal BaseImponible,
    decimal Igv,
    decimal Total);

public record LibroResponseDto(
    LibroContableDto Libro,
    List<LibroDetalleDto> Detalles);
