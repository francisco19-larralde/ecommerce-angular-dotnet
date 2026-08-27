using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> ObtenerTodosAsync();
    Task<ProductoDto?> ObtenerPorIdAsync(int id);
    Task<PaginacionDto<ProductoDto>> ObtenerPaginadoAsync(
        int pagina, int tamanioPagina, int? categoriaId, string? busqueda);
    Task<ResultadoOperacion<ProductoDto>> CrearAsync(CrearProductoDto dto);
    Task<ResultadoOperacion> ActualizarAsync(int id, CrearProductoDto dto);
    Task<ResultadoOperacion<ProductoDto>> ActualizarEstadoAsync(int id, ActualizarEstadoProductoDto dto);
    Task<IEnumerable<ProductoDto>> BuscarAsync(string termino);
    Task<ResultadoOperacion> EliminarAsync(int id);
    Task<PaginacionDto<ProductoDto>> ObtenerCatalogoAsync(FiltroCatalogoDto filtro);
    Task<IEnumerable<string>> ObtenerTallesDisponiblesAsync();
}