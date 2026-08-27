namespace Ecommerce.Api.Models;

public class OrdenItem
{
    public int Id { get; set; }

    public int OrdenId { get; set; }
    public Orden? Orden { get; set; }

    // Referencias, para poder navegar al producto si todavía existe
    public int ProductoId { get; set; }
    public int? VarianteId { get; set; }

    // Snapshot: estos datos NO cambian aunque el producto se edite o elimine después
    public required string ProductoNombre { get; set; }
    public string? Talle { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
}