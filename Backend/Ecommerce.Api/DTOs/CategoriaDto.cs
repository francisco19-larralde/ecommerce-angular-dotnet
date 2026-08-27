namespace Ecommerce.Api.DTOs;

// La que ya teníamos, para selects de formularios (todas las categorías)
public class CategoriaDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
}

// Para el Home: solo las que el admin marcó como visibles, con su orden
public class CategoriaHomeDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
}

// Para la pantalla de administración de categorías
public class CategoriaAdminDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public bool MostrarEnHome { get; set; }
    public int Orden { get; set; }
    public int CantidadProductos { get; set; }
}

public class CrearCategoriaDto
{
    public required string Nombre { get; set; }
    public bool MostrarEnHome { get; set; } = true;
    public int Orden { get; set; } = 0;
}