using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.DTOs;
using Ecommerce.Api.Services;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // Público: todas las categorías, para selects de formularios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        return Ok(await _categoriaService.ObtenerTodasAsync());
    }

    // Público: solo las visibles en Home, ordenadas
    [HttpGet("home")]
    public async Task<ActionResult<IEnumerable<CategoriaHomeDto>>> GetCategoriasHome()
    {
        return Ok(await _categoriaService.ObtenerParaHomeAsync());
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<CategoriaAdminDto>>> GetCategoriasAdmin()
    {
        return Ok(await _categoriaService.ObtenerParaAdminAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoriaAdminDto>> CrearCategoria(CrearCategoriaDto dto)
    {
        var resultado = await _categoriaService.CrearAsync(dto);
        if (!resultado.Exito)
        {
            return BadRequest(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> EliminarCategoria(int id)
    {
        var resultado = await _categoriaService.EliminarAsync(id);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoriaAdminDto>> ActualizarCategoria(int id, CrearCategoriaDto dto)
    {
        var resultado = await _categoriaService.ActualizarAsync(id, dto);
        if (!resultado.Exito)
        {
            return NotFound(new { mensaje = resultado.MensajeError });
        }
        return Ok(resultado.Datos);
    }
}