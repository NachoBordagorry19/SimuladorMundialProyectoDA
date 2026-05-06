using Dominio.Clases;
using Dominio.Enums;

namespace Servicios.Modelo;

public class PartidoDTO
{
    public DateTime Fecha { get; set; }
    public EstadioDTO Estadio { get; set; }
    public EquipoDTO equipoLocal { get; set; }
    public EquipoDTO equipoVisitante {get; set;}
    public Fase fase { get; set; }
    public EstadoPartido estadoPartido { get; set; }
    public int golesLocal { get; set; }
    public int golesVisitante { get; set; }
}