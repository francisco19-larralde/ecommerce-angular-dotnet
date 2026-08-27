using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync();
    Task<IEnumerable<CategoriaHomeDto>> ObtenerParaHomeAsync();
    Task<IEnumerable<CategoriaAdminDto>> ObtenerParaAdminAsync();
    Task<ResultadoOperacion<CategoriaAdminDto>> CrearAsync(CrearCategoriaDto dto);

    Task<ResultadoOperacion<CategoriaAdminDto>> EliminarAsync(int id);
    Task<ResultadoOperacion<CategoriaAdminDto>> ActualizarAsync(int id, CrearCategoriaDto dto);
}