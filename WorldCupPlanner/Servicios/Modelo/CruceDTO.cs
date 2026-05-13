using Dominio.Enums;

namespace Servicios.Modelo;

public class CruceDTO
{
    public string Codigo { get; set; } = string.Empty;

    public Fase Fase { get; set; }

    public PosicionEquipoDTO EquipoLocal { get; set; } = new PosicionEquipoDTO();

    public PosicionEquipoDTO EquipoVisitante { get; set; } = new PosicionEquipoDTO();

    public string ReferenciaLocal { get; set; } = string.Empty;

    public string ReferenciaVisitante { get; set; } = string.Empty;
}