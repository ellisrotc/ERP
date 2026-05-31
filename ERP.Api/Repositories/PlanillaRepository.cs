using ERP.Api.Data;
using ERP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Repositories;

public class PlanillaRepository(ErpDbContext db) : IPlanillaRepository
{
    public async Task<List<Planilla>> GetByPeriodoAsync(string periodo) =>
        await db.Planillas
            .Include(p => p.Empleado)
            .Include(p => p.Detalle)
            .Where(p => p.Periodo == periodo)
            .ToListAsync();

    public async Task<Planilla?> GetByIdWithDetailAsync(int id) =>
        await db.Planillas
            .Include(p => p.Empleado)
            .Include(p => p.Detalle)
            .FirstOrDefaultAsync(p => p.IdPlanilla == id);
}
