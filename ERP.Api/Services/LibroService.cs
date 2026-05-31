using ClosedXML.Excel;
using ERP.Api.Data;
using ERP.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Services;

public interface ILibroService
{
    Task<(LibroContableDto libro, List<LibroDetalleDto> detalles)> GenerarVentasAsync(string periodo, int idUsuario);
    Task<(LibroContableDto libro, List<LibroDetalleDto> detalles)> GenerarComprasAsync(string periodo, int idUsuario);
    Task<byte[]> ExportarExcelVentasAsync(string periodo, int idUsuario);
}

public class LibroService(ErpDbContext db) : ILibroService
{
    public async Task<(LibroContableDto, List<LibroDetalleDto>)> GenerarVentasAsync(string periodo, int idUsuario)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT sp_generar_libro_ventas({periodo}, {idUsuario})");
        var libro = await db.LibrosContables
            .Where(l => l.Tipo == "VENTAS" && l.Periodo == periodo)
            .OrderByDescending(l => l.IdLibro)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"No se generó el libro de ventas para {periodo}");
        return await GetLibroDetalle(libro.IdLibro);
    }

    public async Task<(LibroContableDto, List<LibroDetalleDto>)> GenerarComprasAsync(string periodo, int idUsuario)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT sp_generar_libro_compras({periodo}, {idUsuario})");
        var libro = await db.LibrosContables
            .Where(l => l.Tipo == "COMPRAS" && l.Periodo == periodo)
            .OrderByDescending(l => l.IdLibro)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException($"No se generó el libro de compras para {periodo}");
        return await GetLibroDetalle(libro.IdLibro);
    }

    public async Task<byte[]> ExportarExcelVentasAsync(string periodo, int idUsuario)
    {
        var (libro, detalles) = await GenerarVentasAsync(periodo, idUsuario);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Libro Ventas");

        ws.Cell(1, 1).Value = "REGISTRO DE VENTAS UNAJ";
        ws.Cell(2, 1).Value = $"Período: {periodo}";
        ws.Range(1, 1, 1, 6).Merge().Style.Font.Bold = true;

        var headers = new[] { "N°", "Número", "RUC", "Razón Social", "Base Imponible", "IGV", "Total" };
        for (var i = 0; i < headers.Length; i++)
        {
            ws.Cell(4, i + 1).Value = headers[i];
            ws.Cell(4, i + 1).Style.Font.Bold = true;
            ws.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        }

        for (var i = 0; i < detalles.Count; i++)
        {
            var d = detalles[i];
            var row = i + 5;
            ws.Cell(row, 1).Value = i + 1;
            ws.Cell(row, 2).Value = d.Numero;
            ws.Cell(row, 3).Value = d.Ruc;
            ws.Cell(row, 4).Value = d.RazonSocial;
            ws.Cell(row, 5).Value = d.BaseImponible;
            ws.Cell(row, 6).Value = d.Igv;
            ws.Cell(row, 7).Value = d.Total;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    private async Task<(LibroContableDto, List<LibroDetalleDto>)> GetLibroDetalle(int idLibro)
    {
        var libro = await db.LibrosContables.FindAsync(idLibro)
            ?? throw new KeyNotFoundException("Libro no encontrado");

        var detalles = await db.ComprobantesLibro
            .Include(cl => cl.Comprobante)
            .Where(cl => cl.IdLibro == idLibro)
            .Select(cl => new LibroDetalleDto(
                cl.IdComprobante,
                cl.Comprobante.Numero,
                cl.Comprobante.RazonSocial ?? string.Empty,
                cl.Comprobante.Ruc,
                cl.BaseImponible ?? 0,
                cl.Comprobante.Igv,
                cl.Comprobante.Monto))
            .ToListAsync();

        var libroDto = new LibroContableDto(
            libro.IdLibro, libro.Tipo, libro.Periodo, libro.FechaGeneracion, libro.Estado);

        return (libroDto, detalles);
    }
}
