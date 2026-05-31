using ERP.Api.Services;
using ERP.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/empleados")]
[Authorize(Roles = "Admin,RRHH")]
public class EmpleadoController(IEmpleadoService empleadoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EmpleadoDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var lista = await empleadoService.GetAllAsync(page, pageSize);
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        var emp = await empleadoService.GetByIdAsync(id);
        return Ok(emp);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> Create([FromBody] EmpleadoCreateDto dto)
    {
        var created = await empleadoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmpleadoDto>> Update(int id, [FromBody] EmpleadoUpdateDto dto)
    {
        var updated = await empleadoService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await empleadoService.DeleteAsync(id);
        return NoContent();
    }
}
