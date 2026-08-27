using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public class CarritoService : ICarritoService
{
    private readonly AppDbContext _context;

    public CarritoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CarritoDto> ObtenerCarritoAsync(string usuarioId)
    {
        var carrito = await ObtenerOCrearCarritoAsync(usuarioId);
        return await MapearADtoAsync(carrito.Id);
    }

    public async Task<ResultadoOperacion<CarritoDto>> AgregarItemAsync(string usuarioId, AgregarAlCarritoDto dto)
    {
        var producto = await _context.Productos.FindAsync(dto.ProductoId);
        if (producto is null)
        {
            return ResultadoOperacion<CarritoDto>.Fallo("El producto no existe", TipoError.ValidacionNegocio);
        }

        ProductoVariante? variante = null;

        if (producto.TieneVariantes)
        {
            if (dto.VarianteId is null)
            {
                return ResultadoOperacion<CarritoDto>.Fallo(
                    "Este producto requiere seleccionar un talle", TipoError.ValidacionNegocio);
            }

            variante = await _context.ProductoVariantes
                .FirstOrDefaultAsync(v => v.Id == dto.VarianteId && v.ProductoId == dto.ProductoId);

            if (variante is null)
            {
                return ResultadoOperacion<CarritoDto>.Fallo("El talle seleccionado no es válido", TipoError.ValidacionNegocio);
            }
        }

        var carrito = await ObtenerOCrearCarritoAsync(usuarioId);
        var stockDisponible = variante?.Stock ?? producto.Stock;

        // La igualdad de "mismo ítem" ahora depende del producto Y del talle
        var itemExistente = carrito.Items.FirstOrDefault(i =>
            i.ProductoId == dto.ProductoId && i.VarianteId == dto.VarianteId);

        var cantidadTotal = (itemExistente?.Cantidad ?? 0) + dto.Cantidad;

        if (cantidadTotal > stockDisponible)
        {
            var detalleTalle = variante is not null ? $" (talle {variante.Talle})" : "";
            return ResultadoOperacion<CarritoDto>.Fallo(
                $"Solo hay {stockDisponible} unidades disponibles de \"{producto.Nombre}\"{detalleTalle}",
                TipoError.ValidacionNegocio);
        }

        if (itemExistente is not null)
        {
            itemExistente.Cantidad = cantidadTotal;
        }
        else
        {
            _context.CarritoItems.Add(new CarritoItem
            {
                CarritoId = carrito.Id,
                ProductoId = dto.ProductoId,
                VarianteId = dto.VarianteId,
                Cantidad = dto.Cantidad
            });
        }

        await _context.SaveChangesAsync();

        var carritoActualizado = await MapearADtoAsync(carrito.Id);
        return ResultadoOperacion<CarritoDto>.Ok(carritoActualizado);
    }

    public async Task<ResultadoOperacion<CarritoDto>> ActualizarCantidadAsync(string usuarioId, int itemId, ActualizarCantidadDto dto)
    {
        var item = await _context.CarritoItems
            .Include(i => i.Carrito)
            .Include(i => i.Producto)
            .Include(i => i.Variante)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item is null || item.Carrito!.UsuarioId != usuarioId)
        {
            return ResultadoOperacion<CarritoDto>.Fallo("El ítem no existe en tu carrito", TipoError.NoEncontrado);
        }

        var stockDisponible = item.Variante?.Stock ?? item.Producto!.Stock;

        if (dto.Cantidad > stockDisponible)
        {
            return ResultadoOperacion<CarritoDto>.Fallo(
                $"Solo hay {stockDisponible} unidades disponibles", TipoError.ValidacionNegocio);
        }

        item.Cantidad = dto.Cantidad;
        await _context.SaveChangesAsync();

        var carritoActualizado = await MapearADtoAsync(item.CarritoId);
        return ResultadoOperacion<CarritoDto>.Ok(carritoActualizado);
    }

    public async Task<ResultadoOperacion<CarritoDto>> EliminarItemAsync(string usuarioId, int itemId)
    {
        var item = await _context.CarritoItems
            .Include(i => i.Carrito)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item is null || item.Carrito!.UsuarioId != usuarioId)
        {
            return ResultadoOperacion<CarritoDto>.Fallo(
                "El ítem no existe en tu carrito", TipoError.NoEncontrado);
        }

        var carritoId = item.CarritoId;
        _context.CarritoItems.Remove(item);
        await _context.SaveChangesAsync();

        var carritoActualizado = await MapearADtoAsync(carritoId);
        return ResultadoOperacion<CarritoDto>.Ok(carritoActualizado);
    }

    public async Task<ResultadoOperacion> VaciarCarritoAsync(string usuarioId)
    {
        var carrito = await ObtenerOCrearCarritoAsync(usuarioId);

        _context.CarritoItems.RemoveRange(carrito.Items);
        await _context.SaveChangesAsync();

        return ResultadoOperacion.Ok();
    }



    private async Task<Carrito> ObtenerOCrearCarritoAsync(string usuarioId)
    {
        var carrito = await _context.Carritos
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

        if (carrito is not null)
        {
            return carrito;
        }

        var nuevoCarrito = new Carrito { UsuarioId = usuarioId };
        _context.Carritos.Add(nuevoCarrito);
        await _context.SaveChangesAsync();

        return nuevoCarrito;
    }

    private async Task<CarritoDto> MapearADtoAsync(int carritoId)
    {
        var items = await _context.CarritoItems
            .Include(i => i.Producto)
            .Include(i => i.Variante)
            .Where(i => i.CarritoId == carritoId)
            .Select(i => new CarritoItemDto
            {
                Id = i.Id,
                ProductoId = i.ProductoId,
                ProductoNombre = i.Producto!.Nombre,
                ProductoImagenUrl = i.Producto.ImagenUrl,
                PrecioUnitario = i.Producto.Precio,
                Cantidad = i.Cantidad,
                Subtotal = i.Producto.Precio * i.Cantidad,
                StockDisponible = i.Variante != null ? i.Variante.Stock : i.Producto.Stock,
                VarianteId = i.VarianteId,
                Talle = i.Variante != null ? i.Variante.Talle : null
            })
            .ToListAsync();

        return new CarritoDto { Id = carritoId, Items = items, Total = items.Sum(i => i.Subtotal) };
    }
}