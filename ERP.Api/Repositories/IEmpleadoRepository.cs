using ERP.Api.Models;

namespace ERP.Api.Repositories;

public interface IEmpleadoRepository
{
    Task<List<Empleado>> GetAllAsync(int page, int pageSize);
    Task<Empleado?> GetByIdAsync(int id);
    Task<Empleado> CreateAsync(Empleado empleado);
    Task<Empleado> UpdateAsync(Empleado empleado);
    Task SoftDeleteAsync(int id);
    Task<List<Empleado>> GetActivosAsync();
}
