using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public required string Password { get; set; }
}