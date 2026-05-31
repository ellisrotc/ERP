using ERP.Api.Models;
using ERP.Api.Repositories;
using ERP.Shared.DTOs;

namespace ERP.Api.Services;

public interface IEmpleadoService
{
    Task<List<EmpleadoDto>> GetAllAsync(int page, int pageSize);
    Task<EmpleadoDto> GetByIdAsync(int id);
    Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto);
    Task<EmpleadoDto> UpdateAsync(int id, EmpleadoUpdateDto dto);
    Task DeleteAsync(int id);
}

public class EmpleadoService(IEmpleadoRepository repo) : IEmpleadoService
{
    public async Task<List<EmpleadoDto>> GetAllAsync(int page, int pageSize)
    {
        var empleados = await repo.GetAllAsync(page, pageSize);
        return empleados.Select(ToDto).ToList();
    }

    public async Task<EmpleadoDto> GetByIdAsync(int id)
    {
        var emp = await repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Empleado {id} no encontrado");
        return ToDto(emp);
    }

    public async Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto)
    {
        var emp = new Empleado
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Dni = dto.Dni,
            Cargo = dto.Cargo,
            SalarioBase = dto.SalarioBase,
            IdTipoDescuento = dto.IdTipoDescuento
        };
        var created = await repo.CreateAsync(emp);
        return await GetByIdAsync(created.IdEmpleado);
    }

    public async Task<EmpleadoDto> UpdateAsync(int id, EmpleadoUpdateDto dto)
    {
        var emp = await repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Empleado {id} no encontrado");

        emp.Nombre = dto.Nombre;
        emp.Apellido = dto.Apellido;
        emp.Cargo = dto.Cargo;
        emp.SalarioBase = dto.SalarioBase;
        emp.IdTipoDescuento = dto.IdTipoDescuento;

        await repo.UpdateAsync(emp);
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id) => await repo.SoftDeleteAsync(id);

    private static EmpleadoDto ToDto(Empleado e) => new(
        e.IdEmpleado, e.Nombre, e.Apellido, e.Dni,
        e.Cargo ?? string.Empty,
        e.SalarioBase,
        e.TipoDescuento?.Nombre ?? string.Empty,
        e.Activo);
}
