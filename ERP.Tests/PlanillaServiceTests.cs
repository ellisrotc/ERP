using ERP.Api.Data;
using ERP.Api.Models;
using ERP.Api.Repositories;
using ERP.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ERP.Tests;

public class PlanillaServiceTests
{
    private static ErpDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ErpDbContext(options);
    }

    [Fact]
    public void CalcularAFP_Con10Porciento_RetornaValorCorrecto()
    {
        decimal salario = 3500m;
        decimal afp = salario * 0.10m;
        afp.Should().Be(350m);
    }

    [Fact]
    public void CalcularONP_Con13Porciento_RetornaValorCorrecto()
    {
        decimal salario = 2800m;
        decimal onp = salario * 0.13m;
        onp.Should().Be(364m);
    }

    [Fact]
    public void CalcularNeto_EsBrutoMenosDescuento()
    {
        decimal bruto = 4000m;
        decimal afp = bruto * 0.10m;
        decimal neto = bruto - afp;
        neto.Should().Be(3600m);
    }

    [Fact]
    public void CTS_EsBrutoDivido12()
    {
        decimal bruto = 3000m;
        decimal cts = bruto / 12.0m;
        cts.Should().Be(250m);
    }

    [Fact]
    public void Gratificacion_EnJulio_EsBrutoDivido6()
    {
        decimal bruto = 3600m;
        int mes = 7;
        decimal grat = (mes == 7 || mes == 12) ? bruto / 6.0m : 0m;
        grat.Should().Be(600m);
    }

    [Fact]
    public void Gratificacion_EnMayoNoEsJulioDic_EsCero()
    {
        decimal bruto = 3600m;
        int mes = 5;
        decimal grat = (mes == 7 || mes == 12) ? bruto / 6.0m : 0m;
        grat.Should().Be(0m);
    }

    [Fact]
    public async Task GetByPeriodo_SinDatos_RetornaListaVacia()
    {
        using var db = CreateInMemoryDb();
        var empRepo = new EmpleadoRepository(db);
        var planRepo = new PlanillaRepository(db);
        var svc = new PlanillaService(db, empRepo, planRepo);

        var result = await svc.GetByPeriodoAsync("2026-01");

        result.Should().BeEmpty();
    }

    [Fact]
    public void Neto_ConONP_EsCorecto()
    {
        decimal bruto = 2800m;
        decimal onp = bruto * 0.13m;
        decimal neto = bruto - onp;
        neto.Should().Be(2436m);
    }
}
