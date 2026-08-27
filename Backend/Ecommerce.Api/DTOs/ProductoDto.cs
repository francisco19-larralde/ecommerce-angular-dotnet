namespace Ecommerce.Api.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Destacado { get; set; }
    public bool Activo { get; set; }
    public bool TieneVariantes { get; set; }
    public required List<VarianteDto> Variantes { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
}