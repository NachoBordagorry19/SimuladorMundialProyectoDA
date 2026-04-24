using Dominio.Enums;

namespace Dominio.Clases;

public class Partido
{
    public Equipo Local { get; }
    public Equipo Visitante { get; }
    public Estadio Estadio { get; }

    public int GolesLocal { get; private set; }
    public int GolesVisitante { get; private set; }
    public EstadoPartido Estado { get; private set; }

    public Partido(Equipo local, Equipo visitante, Estadio estadio)
    {
        Local = local;
        Visitante = visitante;
        Estadio = estadio;
        Estado = EstadoPartido.Pendiente;
    }

    public void RegistrarResultado(int golesLocal, int golesVisitante)
    {
        if (Estado == EstadoPartido.Jugado)
            throw new InvalidOperationException("El partido ya fue jugado");

        if (golesLocal < 0 || golesVisitante < 0)
            throw new ArgumentException("Los goles no pueden ser negativos");

        GolesLocal = golesLocal;
        GolesVisitante = golesVisitante;
        Estado = EstadoPartido.Jugado;
    }
}