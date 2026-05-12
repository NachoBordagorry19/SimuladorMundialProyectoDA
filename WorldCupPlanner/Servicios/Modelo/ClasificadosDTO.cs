namespace Servicios.Modelo;

public class ClasificadosDTO
{
    public List<PosicionEquipoDTO> Primeros { get; set; } = new List<PosicionEquipoDTO>();

    public List<PosicionEquipoDTO> Segundos { get; set; } = new List<PosicionEquipoDTO>();

    public List<PosicionEquipoDTO> Terceros { get; set; } = new List<PosicionEquipoDTO>();
}