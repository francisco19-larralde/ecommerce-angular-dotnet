using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Services;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class EstadisticasController : ControllerBase
{
    private readonly IEstadisticaService _estadisticaService;

    public EstadisticasController(IEstadisticaService estadisticaService)
    {
        _estadisticaService = estadisticaService;
    }

    [HttpGet("resumen")]
    public async Task<ActionResult<ResumenVentasDto>> GetResumen()
    {
        return Ok(await _estadisticaService.ObtenerResumenAsync());
    }

    [HttpGet("ventas-por-dia")]
    public async Task<ActionResult<IEnumerable<VentaPorDiaDto>>> GetVentasPorDia([FromQuery] int dias = 30)
    {
        return Ok(await _estadisticaService.ObtenerVentasPorDiaAsync(dias));
    }

    [HttpGet("productos-mas-vendidos")]
    public async Task<ActionResult<IEnumerable<ProductoMasVendidoDto>>> GetProductosMasVendidos([FromQuery] int cantidad = 5)
    {
        return Ok(await _estadisticaService.ObtenerProductosMasVendidosAsync(cantidad));
    }
}