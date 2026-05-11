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
    private Fase _fase;
    private EstadoPartido _estado;
    private int _golesLocal;
    private int _golesVisitante;

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
            {
                throw new ArgumentException("La fecha es invalida");
            }
            _fecha = value;
        }
    }

    public Estadio Estadio
    {
        get => _estadio;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("El estadio no puede ser null");
            }

            _estadio = value;
        }
    }

    public Equipo Local
    {
        get => _local;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("El equipo local es obligatorio");
            }

            _local = value;
        }
    }

    public Equipo Visitante
    {
        get => _visitante;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("El equipo visitante es obligatorio");
            }

            if (_local != null && value == _local)
            {
                throw new ArgumentException("Los equipos no pueden ser los mismos");
            }

            _visitante = value;
        }
    }

    public Equipo? Vencedor
    {
        get
        {
            if (GolesLocal > GolesVisitante)
            {
                return Local;
            }

            if (GolesVisitante > GolesLocal)
            {
                return Visitante;
            }

            return null;
        }
    }
    
    public string Grupo { get; set; }

    public Fase Fase
    {
        get => _fase;
        set => _fase = value;
    }

    public EstadoPartido Estado
    {
        get => _estado;
        set => _estado = value;
    }

    public void MarcarComoJugado()
    {
        Estado = EstadoPartido.Jugado;
    }

    public int GolesLocal
    {
        get => _golesLocal;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Los goles no pueden ser negativos");
            }

            _golesLocal = value;
        }
    }

    public int GolesVisitante
    {
        get => _golesVisitante;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Los goles no pueden ser negativos");
            }

            _golesVisitante = value;
        }
    }
    public Partido(DateTime fecha, Estadio estadio, Equipo local, Equipo visitante, Fase fase, int golesLocal, int golesVisitante)
    {
        _contadorId++;
        Id = _contadorId;

        Fecha = fecha;
        Estadio = estadio;
        Local = local;
        Visitante = visitante;
        Fase = fase;
        Estado = EstadoPartido.Pendiente;
        GolesLocal = golesLocal;
        GolesVisitante = golesVisitante;
    }
}