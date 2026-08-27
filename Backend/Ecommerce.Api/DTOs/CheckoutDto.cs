using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.DTOs;

public class CheckoutDto
{
    public string? CuponCodigo { get; set; }

    [Required(ErrorMessage = "El número de tarjeta es obligatorio")]
    [CreditCard(ErrorMessage = "El número de tarjeta no es válido")]
    public required string NumeroTarjeta { get; set; }

    [Required(ErrorMessage = "El nombre en la tarjeta es obligatorio")]
    public required string NombreTitular { get; set; }

    [Required(ErrorMessage = "El vencimiento es obligatorio")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Formato esperado: MM/AA")]
    public required string Vencimiento { get; set; }

    [Required(ErrorMessage = "El CVV es obligatorio")]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "El CVV debe tener 3 o 4 dígitos")]
    public required string Cvv { get; set; }
}