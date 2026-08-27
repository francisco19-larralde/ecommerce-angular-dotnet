using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Services;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarritoController : ControllerBase
{
    private readonly ICarritoService _carritoService;

    public CarritoController(ICarritoService carritoService)
    {
        _carritoService = carritoService;
    }

    // GET: api/carrito
    [HttpGet]
    public async Task<ActionResult<CarritoDto>> ObtenerCarrito()
    {
        var carrito = await _carritoService.ObtenerCarritoAsync(ObtenerUsuarioId());
        return Ok(carrito);
    }

    // POST: api/carrito/items
    [HttpPost("items")]
    public async Task<ActionResult<CarritoDto>> AgregarItem(AgregarAlCarritoDto dto)
    {
        var resultado = await _carritoService.AgregarItemAsync(ObtenerUsuarioId(), dto);

        if (!resultado.Exito)
        {
            return BadRequest(new { mensaje = resultado.MensajeError });
        }

        return Ok(resultado.Datos);
    }

    // PUT: api/carrito/items/5
    [HttpPut("items/{itemId}")]
    public async Task<ActionResult<CarritoDto>> ActualizarCantidad(int itemId, ActualizarCantidadDto dto)
    {
        var resultado = await _carritoService.ActualizarCantidadAsync(ObtenerUsuarioId(), itemId, dto);

        if (!resultado.Exito)
        {
            return resultado.TipoError == TipoError.NoEncontrado
                ? NotFound(new { mensaje = resultado.MensajeError })
                : BadRequest(new { mensaje = resultado.MensajeError });
        }

        return Ok(resultado.Datos);
    }

    // DELETE: api/carrito/items/5
    [HttpDelete("items/{itemId}")]
    public async Task<ActionResult<CarritoDto>> EliminarItem(int itemId)
    {
        var resultado = await _carritoService.EliminarItemAsync(ObtenerUsuarioId(), itemId);

        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }

        return Ok(resultado.Datos);
    }

    // DELETE: api/carrito
    [HttpDelete]
    public async Task<IActionResult> VaciarCarrito()
    {
        await _carritoService.VaciarCarritoAsync(ObtenerUsuarioId());
        return NoContent();
    }


    private string ObtenerUsuarioId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("No se pudo identificar al usuario");
    }
}