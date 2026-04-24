namespace Dominio.Clases;
using Dominio.Enums;

public class Equipo
{
    private string _nombre;
    private Confederacion _confederacion;
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

    public Confederacion Confederacion
    {
        get => _confederacion;
        set
        {
            _confederacion = value;
        }
    }

    public int RankingFifa
    {
        get => _rankingFifa;
        set
        {
            if (value <= 0)
                throw new ArgumentException("El ranking fifa debe ser un número positivo mayor a 0");
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
    public Equipo(string nombre, Confederacion confederacion, int rankingFifa)
    {
        Nombre = nombre;
        Confederacion = confederacion;
        RankingFifa = rankingFifa;
    }
}