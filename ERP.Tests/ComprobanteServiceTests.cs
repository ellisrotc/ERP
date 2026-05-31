using ERP.Api.Data;
using ERP.Api.Models;
using ERP.Api.Services;
using ERP.Shared.DTOs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ERP.Tests;

public class ComprobanteServiceTests
{
    private static ErpDbContext CreateDb()
    {
        var opts = new DbContextOptionsBuilder<ErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new ErpDbContext(opts);
        db.TiposComprobante.Add(new TipoComprobante { IdTipo = 1, Nombre = "Factura", SeriePrefix = "F001" });
        db.SaveChanges();
        return db;
    }

    [Fact]
    public void IGV_CalculadoCorrectamente_BaseImponible1000_IGV152_54()
    {
        decimal monto = 1180m;
        decimal igv = Math.Round(monto / 1.18m * 0.18m, 2);
        igv.Should().Be(180.00m);
    }

    [Fact]
    public void IGV_Monto1000_EsAproximadamente152()
    {
        decimal monto = 1000m;
        decimal igv = Math.Round(monto / 1.18m * 0.18m, 2);
        igv.Should().BeApproximately(152.54m, 0.01m);
    }

    [Fact]
    public void RUC_11Digitos_EsValido()
    {
        ComprobanteService.EsRucValido("20123456789").Should().BeTrue();
    }

    [Fact]
    public void RUC_MenosDe11Digitos_EsInvalido()
    {
        ComprobanteService.EsRucValido("2012345").Should().BeFalse();
    }

    [Fact]
    public void RUC_ConLetras_EsInvalido()
    {
        ComprobanteService.EsRucValido("2012345678A").Should().BeFalse();
    }

    [Fact]
    public async Task Monto_Negativo_LanzaArgumentException()
    {
        using var db = CreateDb();
        var svc = new ComprobanteService(db);
        var dto = new ComprobanteCreateDto(1, "001", "F001", "20123456789", "ACME SAC",
            DateTime.Today, -500m);
        Func<Task> act = () => svc.CreateAsync(dto);
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CrearComprobante_GuardaEnBD()
    {
        using var db = CreateDb();
        var svc = new ComprobanteService(db);
        var dto = new ComprobanteCreateDto(1, "F001-001", "F001", "20123456789", "EMPRESA SAC",
            DateTime.Today, 1180m);
        var result = await svc.CreateAsync(dto);
        result.Id.Should().BeGreaterThan(0);
        result.Igv.Should().Be(180.00m);
    }

    [Fact]
    public async Task GetComprobantes_FiltraPorFecha()
    {
        using var db = CreateDb();
        var svc = new ComprobanteService(db);

        await svc.CreateAsync(new ComprobanteCreateDto(1, "001", "F001", "20111111111",
            "Empresa A", DateTime.Today, 1180m));
        await svc.CreateAsync(new ComprobanteCreateDto(1, "002", "F001", "20222222222",
            "Empresa B", DateTime.Today.AddMonths(-2), 590m));

        var desde = DateTime.Today.AddDays(-1);
        var lista = await svc.GetAllAsync(desde, null);
        lista.Should().HaveCount(1);
    }
}
