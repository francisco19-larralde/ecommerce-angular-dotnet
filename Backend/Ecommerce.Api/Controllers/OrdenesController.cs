using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Services;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdenesController : ControllerBase
{
    private readonly IOrdenService _ordenService;

    public OrdenesController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }

    // POST: api/ordenes/checkout
    [HttpPost("checkout")]
    public async Task<ActionResult<OrdenDto>> Checkout(CheckoutDto dto)
    {
        var resultado = await _ordenService.RealizarCheckoutAsync(ObtenerUsuarioId(), dto);
        if (!resultado.Exito)
        {
            return BadRequest(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }

    // GET: api/ordenes/mis-compras
    [HttpGet("mis-compras")]
    public async Task<ActionResult<IEnumerable<OrdenDto>>> MisCompras()
    {
        return Ok(await _ordenService.ObtenerMisComprasAsync(ObtenerUsuarioId()));
    }

    // GET: api/ordenes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrdenDto>> Detalle(int id)
    {
        var orden = await _ordenService.ObtenerDetalleAsync(ObtenerUsuarioId(), id);
        if (orden is null)
        {
            return NotFound(new { mensaje = "No se encontró la compra" });
        }
        return Ok(orden);
    }

    // GET: api/ordenes/validar-cupon?codigo=BIENVENIDO10
    [HttpGet("validar-cupon")]
    public async Task<ActionResult> ValidarCupon([FromQuery] string codigo)
    {
        var resultado = await _ordenService.ValidarCuponPublicoAsync(codigo);
        if (!resultado.Exito)
        {
            return BadRequest(new { mensaje = resultado.MensajeError });
        }
        return Ok(new { porcentajeDescuento = resultado.Datos });
    }

    private string ObtenerUsuarioId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("No se pudo identificar al usuario");
    }
}