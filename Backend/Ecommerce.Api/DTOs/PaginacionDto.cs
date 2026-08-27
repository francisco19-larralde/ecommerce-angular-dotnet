namespace Ecommerce.Api.DTOs;

public class PaginacionDto<T>
{
    public required List<T> Items { get; set; }
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TotalRegistros { get; set; }
}