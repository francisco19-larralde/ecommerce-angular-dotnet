namespace Ecommerce.Api.Models;

public class Carrito
{
    public int Id { get; set; }

    public required string UsuarioId { get; set; }
    public ApplicationUser? Usuario { get; set; }

    public List<CarritoItem> Items { get; set; } = [];

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}