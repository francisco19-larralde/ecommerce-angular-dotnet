using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre })
            .ToListAsync();
    }

    // Solo las visibles en Home, en el orden que definió el admin
    public async Task<IEnumerable<CategoriaHomeDto>> ObtenerParaHomeAsync()
    {
        return await _context.Categorias
            .Where(c => c.MostrarEnHome)
            .OrderBy(c => c.Orden)
            .Select(c => new CategoriaHomeDto { Id = c.Id, Nombre = c.Nombre })
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoriaAdminDto>> ObtenerParaAdminAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Orden)
            .Select(c => new CategoriaAdminDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                MostrarEnHome = c.MostrarEnHome,
                Orden = c.Orden,
                CantidadProductos = c.Productos.Count
            })
            .ToListAsync();
    }

    public async Task<ResultadoOperacion<CategoriaAdminDto>> CrearAsync(CrearCategoriaDto dto)
    {
        var existe = await _context.Categorias.AnyAsync(c => c.Nombre == dto.Nombre);
        if (existe)
        {
            return ResultadoOperacion<CategoriaAdminDto>.Fallo(
                "Ya existe una categoría con ese nombre", TipoError.ValidacionNegocio);
        }

        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            MostrarEnHome = dto.MostrarEnHome,
            Orden = dto.Orden
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return ResultadoOperacion<CategoriaAdminDto>.Ok(new CategoriaAdminDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            MostrarEnHome = categoria.MostrarEnHome,
            Orden = categoria.Orden,
            CantidadProductos = 0
        });
    }

    public async Task<ResultadoOperacion<CategoriaAdminDto>> ActualizarAsync(int id, CrearCategoriaDto dto)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return ResultadoOperacion<CategoriaAdminDto>.Fallo(
                "La categoría no existe", TipoError.NoEncontrado);
        }

        categoria.Nombre = dto.Nombre;
        categoria.MostrarEnHome = dto.MostrarEnHome;
        categoria.Orden = dto.Orden;

        await _context.SaveChangesAsync();

        return ResultadoOperacion<CategoriaAdminDto>.Ok(new CategoriaAdminDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            MostrarEnHome = categoria.MostrarEnHome,
            Orden = categoria.Orden,
            CantidadProductos = await _context.Productos.CountAsync(p => p.CategoriaId == id)
        });
    }

    public async Task<ResultadoOperacion<CategoriaAdminDto>> EliminarAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return ResultadoOperacion<CategoriaAdminDto>.Fallo(
                "La categoría no existe", TipoError.NoEncontrado);
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return ResultadoOperacion<CategoriaAdminDto>.Ok(new CategoriaAdminDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            MostrarEnHome = categoria.MostrarEnHome,
            Orden = categoria.Orden,
            CantidadProductos = await _context.Productos.CountAsync(p => p.CategoriaId == id)
        });
    }

}