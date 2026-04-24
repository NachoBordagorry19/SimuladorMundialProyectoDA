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

            if (value.Length > 80)
            {
                throw new ArgumentException("El nombre del estadio no puede superar los 81 caracteres");
            }
            _nombre = value;
        }
    }

    public string Ciudad
    {
        get => _ciudad;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre de la ciudad no puede ser vacio");
            }

            if (value.Length > 60)
            {
                throw new ArgumentException("El nombre de la ciudad no puede ser mayor a 60 caracteres");
            }
            _ciudad = value;
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
        Ciudad = ciudad;
        _descripcion = descripcion;
        _capacidadLocativa = capacidadLocativa;
    }
}