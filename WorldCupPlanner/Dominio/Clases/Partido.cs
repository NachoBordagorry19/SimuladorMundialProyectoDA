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
                throw new ArgumentException("La fecha es invalida");

            _fecha = value;
        }
    }

    public Estadio Estadio
    {
        get => _estadio;
        set
        {
            if (value == null)
                throw new ArgumentException("El estadio no puede ser null");
            _estadio = value;
        }
    }

    public Equipo Local
    {
        get => _local;
        set
        {
            if (value == null)
                throw new ArgumentException("El equipo local es requerido");

            _local = value;
        }
    }

    public Equipo Visitante
    {
        get => _visitante;
        set
        {
            if (value == null)
                throw new ArgumentException("El equipo visitante es requerido");

            if (_local != null && value == _local)
                throw new ArgumentException("Los equipos deben ser distintos");

            _visitante = value;
        }
    }

    public Grupo Grupo
    {
        get => _grupo;
        set
        {
            if (value == null)
                throw new ArgumentException("Grupo  es requerido");

            _grupo = value;
        }
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