namespace Ecommerce.Api.DTOs;

public class FiltroCatalogoDto
{
    public int Pagina { get; set; } = 1;
    public int TamanioPagina { get; set; } = 12;
    public int? CategoriaId { get; set; }
    public decimal? PrecioMin { get; set; }
    public decimal? PrecioMax { get; set; }
    public string? Talle { get; set; }
    public string? Busqueda { get; set; }
    public string? OrdenarPor { get; set; }
}