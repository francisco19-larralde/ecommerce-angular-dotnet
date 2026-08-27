using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.DTOs;

public class AgregarAlCarritoDto
{
    [Required]
    public int ProductoId { get; set; }

    public int? VarianteId { get; set; }  // ← nuevo, obligatorio solo si el producto tiene variantes

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Cantidad { get; set; } = 1;
}