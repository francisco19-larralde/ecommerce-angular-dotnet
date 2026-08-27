using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface IOrdenService
{
    Task<ResultadoOperacion<OrdenDto>> RealizarCheckoutAsync(string usuarioId, CheckoutDto dto);
    Task<IEnumerable<OrdenDto>> ObtenerMisComprasAsync(string usuarioId);
    Task<OrdenDto?> ObtenerDetalleAsync(string usuarioId, int ordenId);
    Task<ResultadoOperacion<int>> ValidarCuponPublicoAsync(string codigo);
}