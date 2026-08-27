using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.DTOs;

namespace Ecommerce.Api.Services;

public class EstadisticaService : IEstadisticaService
{
    private readonly AppDbContext _context;

    public EstadisticaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResumenVentasDto> ObtenerResumenAsync()
    {
        var ordenesPagadas = _context.Ordenes.Where(o => o.Estado == EstadoOrden.Pagada);

        var cantidadOrdenes = await ordenesPagadas.CountAsync();
        var ingresosTotales = cantidadOrdenes > 0 ? await ordenesPagadas.SumAsync(o => o.Total) : 0;
        var productosVendidos = await _context.OrdenItems
            .Where(i => i.Orden!.Estado == EstadoOrden.Pagada)
            .SumAsync(i => (int?)i.Cantidad) ?? 0;

        return new ResumenVentasDto
        {
            IngresosTotales = ingresosTotales,
            CantidadOrdenes = cantidadOrdenes,
            TicketPromedio = cantidadOrdenes > 0 ? Math.Round(ingresosTotales / cantidadOrdenes, 2) : 0,
            ProductosVendidos = productosVendidos
        };
    }

    public async Task<IEnumerable<VentaPorDiaDto>> ObtenerVentasPorDiaAsync(int dias)
    {
        var desde = DateTime.UtcNow.Date.AddDays(-dias + 1);

        var agrupado = await _context.Ordenes
            .Where(o => o.Estado == EstadoOrden.Pagada && o.Fecha >= desde)
            .GroupBy(o => o.Fecha.Date)
            .Select(g => new VentaPorDiaDto
            {
                Fecha = g.Key.ToString("yyyy-MM-dd"),
                Total = g.Sum(o => o.Total),
                CantidadOrdenes = g.Count()
            })
            .ToListAsync();

        // Completamos los días sin ventas con 0, para que el gráfico no tenga huecos
        var resultado = new List<VentaPorDiaDto>();
        for (var fecha = desde; fecha <= DateTime.UtcNow.Date; fecha = fecha.AddDays(1))
        {
            var clave = fecha.ToString("yyyy-MM-dd");
            var existente = agrupado.FirstOrDefault(v => v.Fecha == clave);
            resultado.Add(existente ?? new VentaPorDiaDto { Fecha = clave, Total = 0, CantidadOrdenes = 0 });
        }

        return resultado;
    }

    public async Task<IEnumerable<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(int cantidad)
    {
        return await _context.OrdenItems
            .Where(i => i.Orden!.Estado == EstadoOrden.Pagada)
            .GroupBy(i => i.ProductoNombre)
            .Select(g => new ProductoMasVendidoDto
            {
                Nombre = g.Key,
                CantidadVendida = g.Sum(i => i.Cantidad),
                IngresosGenerados = g.Sum(i => i.Subtotal)
            })
            .OrderByDescending(p => p.CantidadVendida)
            .Take(cantidad)
            .ToListAsync();
    }
}