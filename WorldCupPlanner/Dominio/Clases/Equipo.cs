namespace Dominio.Clases;

public class Equipo
{
    private string _nombre;
    private string _confederacion;
    private int _rankingFifa;

    public String Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre del equipo no puede ser vacio");
            } else if (value.Length > 60)
            {
                throw new ArgumentException("El nombre no puede superar los 60 caracteres");
            }
            _nombre = value;
        }
    }

    public string Confederacion
    {
        get => _confederacion;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre de la confederacion no puede ser vacio");
            } else if (!string.Equals(value, "AFC", StringComparison.OrdinalIgnoreCase)
                       && !string.Equals(value, "CAF", StringComparison.OrdinalIgnoreCase)
                       && !string.Equals(value, "CONCACAF", StringComparison.OrdinalIgnoreCase)
                       && !string.Equals(value, "CONMEBOL", StringComparison.OrdinalIgnoreCase)
                       && !string.Equals(value, "OFC", StringComparison.OrdinalIgnoreCase)
                       && !string.Equals(value, "UEFA", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "El nombre de la confederacion debe ser de los reconocidos: AFC, CAF, CONCACAF, CONMEBOL, OFC, UEFA");
            }
            _confederacion = value;
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
    public Equipo(string nombre, string condeferacion, int rankingFifa)
    {
        Nombre = nombre;
        Confederacion = condeferacion;
        _rankingFifa = rankingFifa;
    }
}