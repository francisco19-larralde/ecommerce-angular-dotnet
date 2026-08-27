namespace Ecommerce.Api.DTOs;

public class ResumenVentasDto
{
    public decimal IngresosTotales { get; set; }
    public int CantidadOrdenes { get; set; }
    public decimal TicketPromedio { get; set; }
    public int ProductosVendidos { get; set; }
}

public class VentaPorDiaDto
{
    public required string Fecha { get; set; } // "yyyy-MM-dd"
    public decimal Total { get; set; }
    public int CantidadOrdenes { get; set; }
}

public class ProductoMasVendidoDto
{
    public required string Nombre { get; set; }
    public int CantidadVendida { get; set; }
    public decimal IngresosGenerados { get; set; }
}