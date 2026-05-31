using ERP.Api.Data;
using ERP.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Repositories;

public class EmpleadoRepository(ErpDbContext db) : IEmpleadoRepository
{
    public async Task<List<Empleado>> GetAllAsync(int page, int pageSize) =>
        await db.Empleados
            .Include(e => e.TipoDescuento)
            .OrderBy(e => e.Apellido)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Empleado?> GetByIdAsync(int id) =>
        await db.Empleados.Include(e => e.TipoDescuento).FirstOrDefaultAsync(e => e.IdEmpleado == id);

    public async Task<Empleado> CreateAsync(Empleado empleado)
    {
        db.Empleados.Add(empleado);
        await db.SaveChangesAsync();
        return empleado;
    }

    public async Task<Empleado> UpdateAsync(Empleado empleado)
    {
        db.Empleados.Update(empleado);
        await db.SaveChangesAsync();
        return empleado;
    }

    public async Task SoftDeleteAsync(int id)
    {
        var emp = await db.Empleados.FindAsync(id)
            ?? throw new KeyNotFoundException($"Empleado {id} no encontrado");
        emp.Activo = false;
        await db.SaveChangesAsync();
    }

    public async Task<List<Empleado>> GetActivosAsync() =>
        await db.Empleados
            .Include(e => e.TipoDescuento)
            .Where(e => e.Activo)
            .ToListAsync();
}
