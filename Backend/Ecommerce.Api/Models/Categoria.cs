namespace Ecommerce.Api.Models;

public class Categoria
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public bool MostrarEnHome { get; set; } = true;  // ← nuevo
    public int Orden { get; set; } = 0;

    public List<Producto> Productos { get; set; } = [];
}