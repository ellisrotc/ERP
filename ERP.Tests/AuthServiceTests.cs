using ERP.Api.Data;
using ERP.Api.Helpers;
using ERP.Api.Models;
using ERP.Api.Services;
using ERP.Shared.DTOs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ERP.Tests;

public class AuthServiceTests
{
    private static ErpDbContext CreateDb()
    {
        var opts = new DbContextOptionsBuilder<ErpDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ErpDbContext(opts);
    }

    private static IConfiguration GetConfig()
    {
        var dict = new Dictionary<string, string?>
        {
            ["Jwt:Secret"] = "TestSecret_MustBe32CharsOrMore_512bits!XYZ",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience"
        };
        return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
    }

    private static (ErpDbContext db, AuthService svc) CreateSvc()
    {
        var db = CreateDb();
        var rol = new Rol { IdRol = 1, NombreRol = "Admin" };
        db.Roles.Add(rol);
        var hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        db.Usuarios.Add(new Usuario
        {
            IdUsuario = 1, Username = "admin", PasswordHash = hash,
            NombreCompleto = "Admin Test", IdRol = 1, Rol = rol, Activo = true
        });
        db.SaveChanges();

        var cfg = GetConfig();
        var jwt = new JwtHelper(cfg);
        return (db, new AuthService(db, jwt));
    }

    [Fact]
    public async Task Login_CredencialesCorrectas_DevuelveToken()
    {
        var (db, svc) = CreateSvc();
        var resp = await svc.LoginAsync(new LoginRequest("admin", "Admin123!"));
        resp.AccessToken.Should().NotBeNullOrEmpty();
        resp.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_PasswordIncorrecto_LanzaUnauthorized()
    {
        var (db, svc) = CreateSvc();
        Func<Task> act = () => svc.LoginAsync(new LoginRequest("admin", "WrongPassword"));
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Login_UsuarioInexistente_LanzaUnauthorized()
    {
        var (db, svc) = CreateSvc();
        Func<Task> act = () => svc.LoginAsync(new LoginRequest("noexiste", "Admin123!"));
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task JWT_ContieneRolCorrecto()
    {
        var (db, svc) = CreateSvc();
        var resp = await svc.LoginAsync(new LoginRequest("admin", "Admin123!"));
        resp.Role.Should().Be("Admin");
    }
}
