namespace Ecommerce.Api.Models;

public class Producto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Destacado { get; set; }
    public bool Activo { get; set; } = true;
    public bool TieneVariantes { get; set; } = false;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public List<ProductoVariante> Variantes { get; set; } = [];
}