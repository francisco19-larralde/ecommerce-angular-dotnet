namespace Ecommerce.Api.DTOs;

public class CarritoDto
{
    public int Id { get; set; }
    public required List<CarritoItemDto> Items { get; set; }
    public decimal Total { get; set; }
}

public class CarritoItemDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public required string ProductoNombre { get; set; }
    public string? ProductoImagenUrl { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
    public int StockDisponible { get; set; }
    public int? VarianteId { get; set; }   // ← nuevo
    public string? Talle { get; set; }     // ← nuevo
}