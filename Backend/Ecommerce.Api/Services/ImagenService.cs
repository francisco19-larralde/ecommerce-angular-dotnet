using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Ecommerce.Api.Data;

namespace Ecommerce.Api.Services;

public class ImagenService : IImagenService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _entorno;

    private static readonly string[] ExtensionesPermitidas = [".jpg", ".jpeg", ".png", ".webp"];
    private const long TamanioMaximoBytes = 5 * 1024 * 1024; // 5 MB

    public ImagenService(AppDbContext context, IWebHostEnvironment entorno)
    {
        _context = context;
        _entorno = entorno;
    }

    public async Task<ResultadoOperacion<string>> SubirImagenAsync(int productoId, IFormFile archivo, string urlBase)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto is null)
        {
            return ResultadoOperacion<string>.Fallo("El producto no existe", TipoError.NoEncontrado);
        }

        if (archivo.Length == 0)
        {
            return ResultadoOperacion<string>.Fallo("El archivo está vacío", TipoError.ValidacionNegocio);
        }

        if (archivo.Length > TamanioMaximoBytes)
        {
            return ResultadoOperacion<string>.Fallo("La imagen no puede pesar más de 5 MB", TipoError.ValidacionNegocio);
        }

        var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            return ResultadoOperacion<string>.Fallo(
                "Formato no permitido. Usá JPG, PNG o WEBP", TipoError.ValidacionNegocio);
        }


        var raiz = _entorno.WebRootPath ?? Path.Combine(_entorno.ContentRootPath, "wwwroot");
        var carpeta = Path.Combine(raiz, "uploads", "productos");
        Directory.CreateDirectory(carpeta);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaFisica = Path.Combine(carpeta, nombreArchivo);

        using (var stream = new FileStream(rutaFisica, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        BorrarArchivoAnteriorSiExiste(producto.ImagenUrl);

        var urlPublica = $"{urlBase}/uploads/productos/{nombreArchivo}";
        producto.ImagenUrl = urlPublica;
        await _context.SaveChangesAsync();

        return ResultadoOperacion<string>.Ok(urlPublica);
    }

    public async Task<ResultadoOperacion> EliminarImagenAsync(int productoId)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto is null)
        {
            return ResultadoOperacion.Fallo("El producto no existe", TipoError.NoEncontrado);
        }

        BorrarArchivoAnteriorSiExiste(producto.ImagenUrl);
        producto.ImagenUrl = null;
        await _context.SaveChangesAsync();

        return ResultadoOperacion.Ok();
    }

    private void BorrarArchivoAnteriorSiExiste(string? urlAnterior)
    {
        if (string.IsNullOrWhiteSpace(urlAnterior)) return;

        var raiz = _entorno.WebRootPath ?? Path.Combine(_entorno.ContentRootPath, "wwwroot");
        var nombreArchivo = Path.GetFileName(urlAnterior);
        var rutaFisica = Path.Combine(raiz, "uploads", "productos", nombreArchivo);

        if (File.Exists(rutaFisica))
        {
            File.Delete(rutaFisica);
        }
    }
}