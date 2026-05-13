namespace Servicios.Modelo;

public class CuadroSegundaFaseDTO
{
    public List<CruceDTO> Dieciseisavos { get; set; } = new List<CruceDTO>();

    public List<CruceDTO> Octavos { get; set; } = new List<CruceDTO>();

    public List<CruceDTO> Cuartos { get; set; } = new List<CruceDTO>();

    public List<CruceDTO> Semifinales { get; set; } = new List<CruceDTO>();

    public List<CruceDTO> PartidosFinales { get; set; } = new List<CruceDTO>();
}