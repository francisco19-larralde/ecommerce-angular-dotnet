using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface IVarianteService
{
    Task<IEnumerable<VarianteDto>> ObtenerPorProductoAsync(int productoId);
    Task<ResultadoOperacion<VarianteDto>> CrearAsync(int productoId, CrearVarianteDto dto);
    Task<ResultadoOperacion<VarianteDto>> ActualizarStockAsync(int varianteId, ActualizarStockVarianteDto dto);
    Task<ResultadoOperacion> EliminarAsync(int varianteId);
}