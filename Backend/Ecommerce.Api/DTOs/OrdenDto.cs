namespace Ecommerce.Api.DTOs;

public class OrdenItemDto
{
    public int ProductoId { get; set; }
    public required string ProductoNombre { get; set; }
    public string? Talle { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
}

public class OrdenDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public required string Estado { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public string? CuponCodigo { get; set; }
    public string? UltimosDigitosTarjeta { get; set; }
    public required List<OrdenItemDto> Items { get; set; }
}