namespace Ecommerce.Api.DTOs;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public DateTime Expiracion { get; set; }
    public required string Nombre { get; set; }
    public required string Email { get; set; }
    public required List<string> Roles { get; set; }
}