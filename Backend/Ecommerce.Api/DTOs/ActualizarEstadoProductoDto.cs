namespace Ecommerce.Api.DTOs;

// Todos los campos son opcionales (bool?), porque el admin puede
// querer cambiar SOLO el destacado, o SOLO el activo, sin mandar el otro
public class ActualizarEstadoProductoDto
{
    public bool? Destacado { get; set; }
    public bool? Activo { get; set; }
}