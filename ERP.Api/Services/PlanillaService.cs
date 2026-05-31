using ERP.Api.Data;
using ERP.Api.Repositories;
using ERP.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ERP.Api.Services;

public interface IPlanillaService
{
    Task CalcularPeriodoAsync(string periodo);
    Task<List<PlanillaResultDto>> GetByPeriodoAsync(string periodo);
    Task<byte[]> GenerarPdfAsync(int idPlanilla);
}

public class PlanillaService(ErpDbContext db, IEmpleadoRepository empleadoRepo, IPlanillaRepository planillaRepo)
    : IPlanillaService
{
    public async Task CalcularPeriodoAsync(string periodo)
    {
        var empleados = await empleadoRepo.GetActivosAsync();
        foreach (var emp in empleados)
        {
            await db.Database.ExecuteSqlRawAsync(
                "SELECT sp_calcular_planilla({0}, {1})", periodo, emp.IdEmpleado);
        }
    }

    public async Task<List<PlanillaResultDto>> GetByPeriodoAsync(string periodo)
    {
        var planillas = await planillaRepo.GetByPeriodoAsync(periodo);
        return planillas.Select(p => new PlanillaResultDto(
            p.IdPlanilla,
            $"{p.Empleado.Nombre} {p.Empleado.Apellido}",
            p.Periodo,
            p.TotalBruto ?? 0,
            p.TotalDescuentos ?? 0,
            p.TotalNeto ?? 0,
            p.Detalle?.Afp ?? 0,
            p.Detalle?.Onp ?? 0,
            p.Detalle?.Cts ?? 0,
            p.Detalle?.Gratificacion ?? 0)).ToList();
    }

    public async Task<byte[]> GenerarPdfAsync(int idPlanilla)
    {
        var planilla = await planillaRepo.GetByIdWithDetailAsync(idPlanilla)
            ?? throw new KeyNotFoundException($"Planilla {idPlanilla} no encontrada");

        QuestPDF.Settings.License = LicenseType.Community;

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Item().Text("UNIVERSIDAD NACIONAL DE JULIACA")
                        .Bold().FontSize(16).AlignCenter();
                    col.Item().Text("BOLETA DE PAGO").FontSize(14).AlignCenter();
                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Text($"Empleado: {planilla.Empleado.Nombre} {planilla.Empleado.Apellido}");
                    col.Item().Text($"DNI: {planilla.Empleado.Dni}");
                    col.Item().Text($"Cargo: {planilla.Empleado.Cargo}");
                    col.Item().Text($"Período: {planilla.Periodo}");
                    col.Item().PaddingVertical(10).LineHorizontal(1);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);
                            c.RelativeColumn(2);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Text("Concepto").Bold();
                            h.Cell().Text("Monto (S/)").Bold().AlignRight();
                        });

                        AddRow(table, "Salario Bruto", planilla.TotalBruto ?? 0);
                        AddRow(table, "AFP (10%)", planilla.Detalle?.Afp ?? 0);
                        AddRow(table, "ONP (13%)", planilla.Detalle?.Onp ?? 0);
                        AddRow(table, "CTS", planilla.Detalle?.Cts ?? 0);
                        AddRow(table, "Gratificación", planilla.Detalle?.Gratificacion ?? 0);
                        AddRow(table, "NETO A PAGAR", planilla.TotalNeto ?? 0, bold: true);
                    });

                    col.Item().PaddingTop(20).Text($"Fecha de cálculo: {planilla.FechaCalculo:dd/MM/yyyy HH:mm}")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });
        });

        return pdf.GeneratePdf();
    }

    private static void AddRow(TableDescriptor table, string concepto, decimal monto, bool bold = false)
    {
        if (bold)
        {
            table.Cell().Text(concepto).Bold();
            table.Cell().Text($"{monto:N2}").Bold().AlignRight();
        }
        else
        {
            table.Cell().Text(concepto);
            table.Cell().Text($"{monto:N2}").AlignRight();
        }
    }
}
