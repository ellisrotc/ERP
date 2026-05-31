using ERP.Api.Data;
using ERP.Api.Helpers;
using ERP.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshAsync(string refreshToken);
}

public class AuthService(ErpDbContext db, JwtHelper jwt) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Activo);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas");

        var accessToken = jwt.GenerateAccessToken(
            usuario.IdUsuario, usuario.Username, usuario.Rol.NombreRol, usuario.NombreCompleto);
        var refreshToken = JwtHelper.GenerateRefreshToken();

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        usuario.UltimoAcceso = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return new LoginResponse(accessToken, refreshToken, usuario.Rol.NombreRol, usuario.NombreCompleto);
    }

    public async Task<LoginResponse> RefreshAsync(string refreshToken)
    {
        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.Activo);

        if (usuario is null || usuario.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token inválido o expirado");

        var accessToken = jwt.GenerateAccessToken(
            usuario.IdUsuario, usuario.Username, usuario.Rol.NombreRol, usuario.NombreCompleto);
        var newRefresh = JwtHelper.GenerateRefreshToken();

        usuario.RefreshToken = newRefresh;
        usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await db.SaveChangesAsync();

        return new LoginResponse(accessToken, newRefresh, usuario.Rol.NombreRol, usuario.NombreCompleto);
    }
}
