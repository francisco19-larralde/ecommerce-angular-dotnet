namespace Ecommerce.Api.Models;

public class ProductoVariante
{
    public int Id { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public required string Talle { get; set; }  // "S", "M", "38", "40", etc. — texto libre
    public int Stock { get; set; }
    public int Orden { get; set; }               // para mostrarlos ordenados: S, M, L, XL
}