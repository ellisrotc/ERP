namespace ERP.Api.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public Rol Rol { get; set; } = null!;
    public bool Activo { get; set; } = true;
    public DateTime? UltimoAcceso { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
