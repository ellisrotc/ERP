using ERP.Api.Services;
using ERP.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/comprobantes")]
[Authorize(Roles = "Admin,Contador")]
public class ComprobanteController(IComprobanteService comprobanteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ComprobanteDto>>> GetAll(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        var lista = await comprobanteService.GetAllAsync(desde, hasta);
        return Ok(lista);
    }

    [HttpPost]
    public async Task<ActionResult<ComprobanteDto>> Create([FromBody] ComprobanteCreateDto dto)
    {
        var created = await comprobanteService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComprobanteDto>> GetById(int id)
    {
        var comp = await comprobanteService.GetByIdAsync(id);
        return Ok(comp);
    }
}
