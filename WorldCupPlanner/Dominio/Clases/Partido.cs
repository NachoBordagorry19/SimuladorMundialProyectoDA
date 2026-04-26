using Dominio.Enums;

namespace Dominio.Clases;

public class Partido
{
    private static int _contadorId = 0;
    private int _id;
    private DateTime _fecha;
    private Estadio _estadio;
    private Equipo _local;
    private Equipo _visitante;
    private Grupo _grupo;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public DateTime Fecha
    {
        get => _fecha;
        set
        {
            if (value == DateTime.MinValue)
                throw new ArgumentException();

            _fecha = value;
        }
    }

    public Estadio Estadio
    {
        get => _estadio;
        set => _estadio = value;
    }

    public Equipo Local
    {
        get => _local;
        set => _local = value;
    }

    public Equipo Visitante
    {
        get => _visitante;
        set => _visitante = value;
    }

    public Grupo Grupo
    {
        get => _grupo;
        set => _grupo = value;
    }
    
    
    public Partido(DateTime fecha, Estadio estadio, Equipo local, Equipo visitante, Grupo grupo)
    {
        _contadorId++;
        Id = _contadorId;
        
        Fecha = fecha;
        Estadio = estadio;
        Local = local;
        Visitante = visitante;
        Grupo = grupo;
    }
}