using ERP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Services;

public interface IReporteService
{
    Task<object> GetBalanceAsync(string periodo);
    Task<object> GetResultadosAsync(string periodo);
}

public class ReporteService(ErpDbContext db) : IReporteService
{
    public async Task<object> GetBalanceAsync(string periodo)
    {
        var ventas = await db.ComprobantesLibro
            .Include(cl => cl.Libro)
            .Include(cl => cl.Comprobante)
            .Where(cl => cl.Libro.Periodo == periodo && cl.Libro.Tipo == "VENTAS")
            .SumAsync(cl => cl.Comprobante.Monto);

        var compras = await db.ComprobantesLibro
            .Include(cl => cl.Libro)
            .Include(cl => cl.Comprobante)
            .Where(cl => cl.Libro.Periodo == periodo && cl.Libro.Tipo == "COMPRAS")
            .SumAsync(cl => cl.Comprobante.Monto);

        var totalPlanilla = await db.Planillas
            .Where(p => p.Periodo == periodo)
            .SumAsync(p => p.TotalNeto ?? 0);

        return new
        {
            Periodo = periodo,
            Activo = new { CuentasPorCobrar = ventas, Total = ventas },
            Pasivo = new { CuentasPorPagar = compras, Planilla = totalPlanilla, Total = compras + totalPlanilla },
            Patrimonio = ventas - compras - totalPlanilla
        };
    }

    public async Task<object> GetResultadosAsync(string periodo)
    {
        var ingresos = await db.ComprobantesLibro
            .Include(cl => cl.Libro)
            .Include(cl => cl.Comprobante)
            .Where(cl => cl.Libro.Periodo == periodo && cl.Libro.Tipo == "VENTAS")
            .SumAsync(cl => cl.BaseImponible ?? 0);

        var costos = await db.ComprobantesLibro
            .Include(cl => cl.Libro)
            .Include(cl => cl.Comprobante)
            .Where(cl => cl.Libro.Periodo == periodo && cl.Libro.Tipo == "COMPRAS")
            .SumAsync(cl => cl.BaseImponible ?? 0);

        var gastoPersonal = await db.Planillas
            .Where(p => p.Periodo == periodo)
            .SumAsync(p => p.TotalNeto ?? 0);

        var utilidadBruta = ingresos - costos;
        var utilidadNeta = utilidadBruta - gastoPersonal;

        return new
        {
            Periodo = periodo,
            Ingresos = ingresos,
            CostoVentas = costos,
            UtilidadBruta = utilidadBruta,
            GastosPersonal = gastoPersonal,
            UtilidadNeta = utilidadNeta
        };
    }
}
