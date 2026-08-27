using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.DTOs;

public class CrearProductoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(150)]
    public required string Nombre { get; set; }

    public string? Descripcion { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    public string? ImagenUrl { get; set; }
    public bool Destacado { get; set; } = false;
    public bool TieneVariantes { get; set; } = false;  // ← nuevo

    [Required(ErrorMessage = "Debe seleccionar una categoría")]
    public int CategoriaId { get; set; }
}