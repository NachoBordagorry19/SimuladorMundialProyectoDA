namespace Servicios.Modelo;

using Dominio.Enums;

public class IncidenciaDTO
{
    public int Id { get; set; }
    public int IdPartido { get; set; }
    public int IdEquipo { get; set; }
    public TipoIncidencia TipoIncidencia { get; set; }
}