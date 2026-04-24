namespace Dominio.Clases;

public class Equipo
{
    private string _nombre;
    private string _confederacion;
    private string _rankingFifa;

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

    public string RankingFifa
    {
        get => _rankingFifa;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El ranking fifa no puede ser vacio");
            } 
            if (!int.TryParse(value, out var entero))
                throw new ArgumentException("El ranking fifa debe ser un número entero", nameof(RankingFifa));
            if (entero < 0)
                throw new ArgumentException("El ranking fifa debe ser un número no negativo", nameof(RankingFifa));
            _rankingFifa = value;
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
    public Equipo(string nombre, string condeferacion, string rankingFifa)
    {
        Nombre = nombre;
        Confederacion = condeferacion;
        RankingFifa = rankingFifa;
    }
}