namespace Dominio.Clases;

public class Estadio
{
    public string Nombre { get; }
    public string Ciudad { get; }
    public int Capacidad { get; }

    public Estadio(string nombre, string ciudad, int capacidad)
    {
        Nombre = nombre;
        Ciudad = ciudad;
        Capacidad = capacidad;
    }
}