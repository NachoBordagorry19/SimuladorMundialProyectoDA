using Dominio.Clases;
using Dominio.Enums;

namespace Servicios.Modelo;

public class PartidoDTO
{
    public int idPartido { get; set; }
    public DateTime Fecha { get; set; }
    public EstadioDTO Estadio { get; set; } = new EstadioDTO();
    public EquipoDTO equipoLocal { get; set; } = new EquipoDTO();
    public EquipoDTO equipoVisitante { get; set; } = new EquipoDTO();
    public string Grupo { get; set; } = string.Empty;
    public Fase fase { get; set; }
    public EstadoPartido estadoPartido { get; set; }
    public int golesLocal { get; set; }
    public int golesVisitante { get; set; }
}