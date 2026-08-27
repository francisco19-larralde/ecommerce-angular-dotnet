namespace Ecommerce.Api.Models;

public class CarritoItem
{
    public int Id { get; set; }

    public int CarritoId { get; set; }
    public Carrito? Carrito { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int? VarianteId { get; set; }
    public ProductoVariante? Variante { get; set; }
    public int Cantidad { get; set; }
}