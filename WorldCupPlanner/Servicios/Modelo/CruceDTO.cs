using Dominio.Enums;

namespace Servicios.Modelo;

public class CruceDTO
{
    public string Codigo { get; set; }

    public Fase Fase { get; set; }

    public PosicionEquipoDTO EquipoLocal { get; set; }

    public PosicionEquipoDTO EquipoVisitante { get; set; }

    public string ReferenciaLocal { get; set; }

    public string ReferenciaVisitante { get; set; }
}