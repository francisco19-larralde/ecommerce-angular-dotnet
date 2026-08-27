using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface ICarritoService
{
    Task<CarritoDto> ObtenerCarritoAsync(string usuarioId);
    Task<ResultadoOperacion<CarritoDto>> AgregarItemAsync(string usuarioId, AgregarAlCarritoDto dto);
    Task<ResultadoOperacion<CarritoDto>> ActualizarCantidadAsync(string usuarioId, int itemId, ActualizarCantidadDto dto);
    Task<ResultadoOperacion<CarritoDto>> EliminarItemAsync(string usuarioId, int itemId);
    Task<ResultadoOperacion> VaciarCarritoAsync(string usuarioId);
}