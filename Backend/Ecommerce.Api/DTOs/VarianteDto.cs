namespace Ecommerce.Api.DTOs;

public class VarianteDto
{
    public int Id { get; set; }
    public required string Talle { get; set; }
    public int Stock { get; set; }
    public int Orden { get; set; }
}

public class CrearVarianteDto
{
    public required string Talle { get; set; }
    public int Stock { get; set; }
    public int Orden { get; set; } = 0;
}

public class ActualizarStockVarianteDto
{
    public int Stock { get; set; }
}