namespace ERP.Shared.DTOs;

public record LoginRequest(string Username, string Password);

public record LoginResponse(string AccessToken, string RefreshToken, string Role, string NombreCompleto);

public record RefreshRequest(string RefreshToken);
