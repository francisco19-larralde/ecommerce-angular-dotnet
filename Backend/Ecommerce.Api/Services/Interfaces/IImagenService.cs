using Microsoft.AspNetCore.Http;

namespace Ecommerce.Api.Services;

public interface IImagenService
{
    Task<ResultadoOperacion<string>> SubirImagenAsync(int productoId, IFormFile archivo, string urlBase);
    Task<ResultadoOperacion> EliminarImagenAsync(int productoId);
}