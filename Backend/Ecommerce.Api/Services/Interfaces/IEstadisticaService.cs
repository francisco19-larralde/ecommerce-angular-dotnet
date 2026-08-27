using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public interface IEstadisticaService
{
    Task<ResumenVentasDto> ObtenerResumenAsync();
    Task<IEnumerable<VentaPorDiaDto>> ObtenerVentasPorDiaAsync(int dias);
    Task<IEnumerable<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(int cantidad);
}