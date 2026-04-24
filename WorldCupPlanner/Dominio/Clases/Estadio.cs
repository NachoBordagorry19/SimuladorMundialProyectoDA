namespace Dominio.Clases;

public class Estadio
{
    private string _nombre;
    private string _ciudad;
    private string _descripcion;
    private int _capacidadLocativa;

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre no puede ser vacio");
            }
            _nombre = value;
        }
    }
    
    public bool EsVacio(string textoATestear)
    {
        bool vacio = true;
        if (!String.IsNullOrEmpty(textoATestear))
        {
            vacio = false;
        }
        return vacio;
    }
    public Estadio(string nombre, string ciudad, string descripcion, int capacidadLocativa)
    {
        Nombre = nombre;
        _ciudad = ciudad;
        _descripcion = descripcion;
        _capacidadLocativa = capacidadLocativa;
    }
}