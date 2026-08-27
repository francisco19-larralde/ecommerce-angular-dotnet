namespace Ecommerce.Api.Models;

public enum EstadoOrden
{
    Pagada,
    Cancelada
}

public class Orden
{
    public int Id { get; set; }

    public required string UsuarioId { get; set; }
    public ApplicationUser? Usuario { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public EstadoOrden Estado { get; set; } = EstadoOrden.Pagada;

    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }

    public string? CuponCodigo { get; set; }
    public string? UltimosDigitosTarjeta { get; set; }

    public List<OrdenItem> Items { get; set; } = [];
}