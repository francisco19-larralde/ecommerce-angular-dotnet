namespace Ecommerce.Api.Models;

public class CuponDescuento
{
    public int Id { get; set; }
    public required string Codigo { get; set; }
    public int PorcentajeDescuento { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaExpiracion { get; set; }
    public int? UsoMaximo { get; set; }
    public int VecesUsado { get; set; } = 0;
}