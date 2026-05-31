using ERP.Api.Services;
using ERP.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/planillas")]
[Authorize(Roles = "Admin,RRHH")]
public class PlanillaController(IPlanillaService planillaService) : ControllerBase
{
    [HttpPost("calcular")]
    public async Task<IActionResult> Calcular([FromQuery] string periodo)
    {
        if (string.IsNullOrWhiteSpace(periodo))
            return BadRequest("El período es requerido (formato: yyyy-MM)");
        await planillaService.CalcularPeriodoAsync(periodo);
        return Ok(new { mensaje = $"Planilla del período {periodo} calculada exitosamente" });
    }

    [HttpGet]
    public async Task<ActionResult<List<PlanillaResultDto>>> GetByPeriodo([FromQuery] string periodo)
    {
        var lista = await planillaService.GetByPeriodoAsync(periodo);
        return Ok(lista);
    }

    [HttpGet("{id:int}/pdf")]
    public async Task<IActionResult> GetPdf(int id)
    {
        var pdf = await planillaService.GenerarPdfAsync(id);
        return File(pdf, "application/pdf", $"planilla_{id}.pdf");
    }
}
