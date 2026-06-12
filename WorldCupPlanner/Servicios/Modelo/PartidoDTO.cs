using System.ComponentModel.DataAnnotations;
using Dominio.Clases;
using Dominio.Enums;

namespace Servicios.Modelo;

public class PartidoDTO
{
    public int IdPartido { get; set; }
    
    [Required(ErrorMessage = "La fecha es obligatoria")] 
    public DateTime Fecha { get; set; }
    public EstadioDTO Estadio { get; set; } = new EstadioDTO();
    public EquipoDTO EquipoLocal { get; set; } = new EquipoDTO();
    public EquipoDTO EquipoVisitante { get; set; } = new EquipoDTO();
    public string Grupo { get; set; } = string.Empty;
    public Fase Fase { get; set; }
    public EstadoPartido EstadoPartido { get; set; }
    
    [Range(0, 50, ErrorMessage = "Los goles deben estar entre 0 y 50")]
    public int GolesLocal { get; set; }
    
    [Range(0, 50, ErrorMessage = "Los goles deben estar entre 0 y 50")]
    public int GolesVisitante { get; set; }
    public List<TipoIncidencia> IncidenciaEquipoLocal { get; set; } = new List<TipoIncidencia>();
    public List<TipoIncidencia> IncidenciaEquipoVisitante { get; set; } = new List<TipoIncidencia>();
}