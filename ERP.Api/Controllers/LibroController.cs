using System.Security.Claims;
using ERP.Api.Services;
using ERP.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/libros")]
[Authorize(Roles = "Admin,Contador")]
public class LibroController(ILibroService libroService) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpGet("ventas")]
    public async Task<ActionResult<object>> GetVentas([FromQuery] string periodo)
    {
        var (libro, detalles) = await libroService.GenerarVentasAsync(periodo, UserId);
        return Ok(new { libro, detalles });
    }

    [HttpGet("compras")]
    public async Task<ActionResult<object>> GetCompras([FromQuery] string periodo)
    {
        var (libro, detalles) = await libroService.GenerarComprasAsync(periodo, UserId);
        return Ok(new { libro, detalles });
    }

    [HttpGet("ventas/{periodo}/excel")]
    public async Task<IActionResult> ExportarVentasExcel(string periodo)
    {
        var bytes = await libroService.ExportarExcelVentasAsync(periodo, UserId);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"libro_ventas_{periodo}.xlsx");
    }
}
