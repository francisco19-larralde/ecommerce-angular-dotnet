namespace Ecommerce.Api.Services;


public class ResultadoOperacion
{
    public bool Exito { get; set; }
    public string? MensajeError { get; set; }
    public TipoError? TipoError { get; set; }

    public static ResultadoOperacion Ok() => new() { Exito = true };

    public static ResultadoOperacion Fallo(string mensaje, TipoError tipo) =>
        new() { Exito = false, MensajeError = mensaje, TipoError = tipo };
}


public class ResultadoOperacion<T> : ResultadoOperacion
{
    public T? Datos { get; set; }

    public static ResultadoOperacion<T> Ok(T datos) =>
        new() { Exito = true, Datos = datos };

    public static new ResultadoOperacion<T> Fallo(string mensaje, TipoError tipo) =>
        new() { Exito = false, MensajeError = mensaje, TipoError = tipo };
}


public enum TipoError
{
    NoEncontrado,
    ValidacionNegocio
}