using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public class VarianteService : IVarianteService
{
    private readonly AppDbContext _context;

    public VarianteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VarianteDto>> ObtenerPorProductoAsync(int productoId)
    {
        return await _context.ProductoVariantes
            .Where(v => v.ProductoId == productoId)
            .OrderBy(v => v.Orden)
            .Select(v => MapearADto(v))
            .ToListAsync();
    }

    public async Task<ResultadoOperacion<VarianteDto>> CrearAsync(int productoId, CrearVarianteDto dto)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto is null)
        {
            return ResultadoOperacion<VarianteDto>.Fallo("El producto no existe", TipoError.NoEncontrado);
        }

        var talleDuplicado = await _context.ProductoVariantes
            .AnyAsync(v => v.ProductoId == productoId && v.Talle == dto.Talle);

        if (talleDuplicado)
        {
            return ResultadoOperacion<VarianteDto>.Fallo(
                $"Ya existe el talle \"{dto.Talle}\" para este producto", TipoError.ValidacionNegocio);
        }

        var variante = new ProductoVariante
        {
            ProductoId = productoId,
            Talle = dto.Talle,
            Stock = dto.Stock,
            Orden = dto.Orden
        };

        _context.ProductoVariantes.Add(variante);


        producto.TieneVariantes = true;
        await RecalcularStockProductoAsync(productoId);

        await _context.SaveChangesAsync();

        return ResultadoOperacion<VarianteDto>.Ok(MapearADto(variante));
    }

    public async Task<ResultadoOperacion<VarianteDto>> ActualizarStockAsync(int varianteId, ActualizarStockVarianteDto dto)
    {
        var variante = await _context.ProductoVariantes.FindAsync(varianteId);
        if (variante is null)
        {
            return ResultadoOperacion<VarianteDto>.Fallo("El talle no existe", TipoError.NoEncontrado);
        }

        variante.Stock = dto.Stock;
        await _context.SaveChangesAsync();
        await RecalcularStockProductoAsync(variante.ProductoId);

        return ResultadoOperacion<VarianteDto>.Ok(MapearADto(variante));
    }

    public async Task<ResultadoOperacion> EliminarAsync(int varianteId)
    {
        var variante = await _context.ProductoVariantes
            .Include(v => v.Producto)
            .FirstOrDefaultAsync(v => v.Id == varianteId);

        if (variante is null)
        {
            return ResultadoOperacion.Fallo("El talle no existe", TipoError.NoEncontrado);
        }

        var productoId = variante.ProductoId;

        var itemsAfectados = await _context.CarritoItems
                .Where(i => i.VarianteId == varianteId)
                .ToListAsync();

        if (itemsAfectados.Count > 0)
        {
            _context.CarritoItems.RemoveRange(itemsAfectados);
        }

        _context.ProductoVariantes.Remove(variante);
        await _context.SaveChangesAsync();


        var quedanVariantes = await _context.ProductoVariantes.AnyAsync(v => v.ProductoId == productoId);
        if (!quedanVariantes && variante.Producto is not null)
        {
            variante.Producto.TieneVariantes = false;
            variante.Producto.Stock = 0;
            await _context.SaveChangesAsync();
        }
        else
        {
            await RecalcularStockProductoAsync(productoId);
        }

        return ResultadoOperacion.Ok();
    }

    private async Task RecalcularStockProductoAsync(int productoId)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto is null || !producto.TieneVariantes) return;

        producto.Stock = await _context.ProductoVariantes
            .Where(v => v.ProductoId == productoId)
            .SumAsync(v => v.Stock);

        await _context.SaveChangesAsync();
    }

    private static VarianteDto MapearADto(ProductoVariante v)
    {
        return new VarianteDto { Id = v.Id, Talle = v.Talle, Stock = v.Stock, Orden = v.Orden };
    }
}