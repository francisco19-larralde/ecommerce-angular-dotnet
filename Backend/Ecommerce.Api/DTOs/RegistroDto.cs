using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.DTOs;

public class RegistroDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public required string Apellido { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public required string Password { get; set; }
}