namespace Dominio.Clases;

public class Estadio
{
    private string _nombre;
    private string _ciudad;
    private string _descripcion;
    private int _capacidadLocativa;
    
    public Estadio(string nombre, string ciudad, string descripcion, int capacidadLocativa)
    {
        _nombre = nombre;
        _ciudad = ciudad;
        _descripcion = descripcion;
        _capacidadLocativa = capacidadLocativa;
    }
}