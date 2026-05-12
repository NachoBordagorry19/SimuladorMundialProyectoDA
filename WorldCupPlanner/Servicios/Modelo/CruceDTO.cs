namespace Servicios.Modelo;

public class CruceDTO
{
    public string Codigo { get; set; }

    public PosicionEquipoDTO EquipoLocal { get; set; }

    public PosicionEquipoDTO EquipoVisitante { get; set; }
}