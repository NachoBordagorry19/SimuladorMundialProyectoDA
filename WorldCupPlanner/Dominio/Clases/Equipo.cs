namespace Dominio.Clases;
using Dominio.Enums;

public class Equipo
{
    public int id { get; set; }
    private string _nombre = string.Empty;
    private Confederacion _confederacion;
    private int _rankingFifa;
    private int _puntos;
    private int _golesAFavor;
    private int _diferenciaDeGoles;
    public String Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre del equipo no puede ser vacio");
            }
            else if (value.Length > 60)
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
            if (value < 300 || value > 2500)
                throw new ArgumentException("El ranking FIFA debe ser un numero entre 300 y 2500");
            _rankingFifa = value;
        }
    }

    public int Puntos
    {
        get => _puntos;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Los puntos asignados no pueden ser negativos");
            }
            _puntos = value;
        }
    }

    public int GolesAFavor
    {
        get => _golesAFavor;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Los goles a favor no pueden ser negativos");
            }
            _golesAFavor = value;
        }
    }

    public int DiferenciaDeGoles
    {
        get => _diferenciaDeGoles;
        set => _diferenciaDeGoles = value;
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

    public Equipo()
    {
        
    }
    public Equipo(string nombre, Confederacion confederacion, int rankingFifa)
    {
        Nombre = nombre;
        Confederacion = confederacion;
        RankingFifa = rankingFifa;
        Puntos = 0;
        GolesAFavor = 0;
        DiferenciaDeGoles = 0;
    }
}