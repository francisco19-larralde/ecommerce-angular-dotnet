using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Services;
using Ecommerce.Api.Services.Interfaces;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly IVarianteService _varianteService;
    private readonly IImagenService _imagenService;

    public ProductosController(
    IProductoService productoService,
    IVarianteService varianteService,
    IImagenService imagenService)
    {
        _productoService = productoService;
        _varianteService = varianteService;
        _imagenService = imagenService;
    }


    [HttpPost("{productoId}/imagen")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> SubirImagen(int productoId, IFormFile archivo)
    {
        var urlBase = $"{Request.Scheme}://{Request.Host}";
        var resultado = await _imagenService.SubirImagenAsync(productoId, archivo, urlBase);

        if (!resultado.Exito)
        {
            return resultado.TipoError == TipoError.NoEncontrado
                ? NotFound(new { mensaje = resultado.MensajeError })
                : BadRequest(new { mensaje = resultado.MensajeError });
        }

        return Ok(new { imagenUrl = resultado.Datos });
    }


    [HttpDelete("{productoId}/imagen")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarImagen(int productoId)
    {
        var resultado = await _imagenService.EliminarImagenAsync(productoId);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return NoContent();
    }


    [HttpGet("{productoId}/variantes")]
    public async Task<ActionResult<IEnumerable<VarianteDto>>> GetVariantes(int productoId)
    {
        return Ok(await _varianteService.ObtenerPorProductoAsync(productoId));
    }

    // POST: api/productos/5/variantes — solo Admin
    [HttpPost("{productoId}/variantes")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VarianteDto>> CrearVariante(int productoId, CrearVarianteDto dto)
    {
        var resultado = await _varianteService.CrearAsync(productoId, dto);
        if (!resultado.Exito)
        {
            return resultado.TipoError == TipoError.NoEncontrado
                ? NotFound(new { mensaje = resultado.MensajeError })
                : BadRequest(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }

    // PUT: api/productos/variantes/8/stock — solo Admin
    [HttpPut("variantes/{varianteId}/stock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<VarianteDto>> ActualizarStockVariante(int varianteId, ActualizarStockVarianteDto dto)
    {
        var resultado = await _varianteService.ActualizarStockAsync(varianteId, dto);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }

    // DELETE: api/productos/variantes/8 — solo Admin
    [HttpDelete("variantes/{varianteId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarVariante(int varianteId)
    {
        var resultado = await _varianteService.EliminarAsync(varianteId);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
    {
        return Ok(await _productoService.ObtenerTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetProducto(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        if (producto is null)
        {
            return NotFound(new { mensaje = $"No se encontró el producto con id {id}" });
        }
        return Ok(producto);
    }

    // GET: api/productos/buscar?termino=zapa
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Buscar([FromQuery] string termino)
    {
        var resultados = await _productoService.BuscarAsync(termino);
        return Ok(resultados);
    }

    // GET: api/productos/admin?pagina=1&tamanioPagina=10&categoriaId=2&busqueda=zapa
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaginacionDto<ProductoDto>>> GetProductosAdmin(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanioPagina = 10,
        [FromQuery] int? categoriaId = null,
        [FromQuery] string? busqueda = null)
    {
        var resultado = await _productoService.ObtenerPaginadoAsync(pagina, tamanioPagina, categoriaId, busqueda);
        return Ok(resultado);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductoDto>> CrearProducto(CrearProductoDto dto)
    {
        var resultado = await _productoService.CrearAsync(dto);
        if (!resultado.Exito)
        {
            return BadRequest(new { mensaje = resultado.MensajeError });
        }
        return CreatedAtAction(nameof(GetProducto), new { id = resultado.Datos!.Id }, resultado.Datos);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActualizarProducto(int id, CrearProductoDto dto)
    {
        var resultado = await _productoService.ActualizarAsync(id, dto);
        if (!resultado.Exito)
        {
            return resultado.TipoError == TipoError.NoEncontrado
                ? NotFound(new { mensaje = resultado.MensajeError })
                : BadRequest(new { mensaje = resultado.MensajeError });
        }
        return NoContent();
    }

    // PATCH: api/productos/5/estado — actualización parcial, para toggles rápidos
    [HttpPatch("{id}/estado")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductoDto>> ActualizarEstado(int id, ActualizarEstadoProductoDto dto)
    {
        var resultado = await _productoService.ActualizarEstadoAsync(id, dto);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var resultado = await _productoService.EliminarAsync(id);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return NoContent();
    }


    [HttpGet("catalogo")]
    public async Task<ActionResult<PaginacionDto<ProductoDto>>> GetCatalogo([FromQuery] FiltroCatalogoDto filtro)
    {
        return Ok(await _productoService.ObtenerCatalogoAsync(filtro));
    }


    [HttpGet("talles-disponibles")]
    public async Task<ActionResult<IEnumerable<string>>> GetTallesDisponibles()
    {
        return Ok(await _productoService.ObtenerTallesDisponiblesAsync());
    }
}