using ERP.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize(Roles = "Admin,Gerente,Contador")]
public class ReporteController(IReporteService reporteService) : ControllerBase
{
    [HttpGet("balance")]
    public async Task<ActionResult<object>> GetBalance([FromQuery] string periodo)
    {
        var balance = await reporteService.GetBalanceAsync(periodo);
        return Ok(balance);
    }

    [HttpGet("resultados")]
    public async Task<ActionResult<object>> GetResultados([FromQuery] string periodo)
    {
        var resultados = await reporteService.GetResultadosAsync(periodo);
        return Ok(resultados);
    }
}
