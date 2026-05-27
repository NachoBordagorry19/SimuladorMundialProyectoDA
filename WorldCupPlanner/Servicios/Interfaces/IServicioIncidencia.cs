using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioIncidencia
{
    public void AgregarIncidencia(IncidenciaDTO incidencia);
    public List<IncidenciaDTO> ObtenerIncidenciasPorPartido(int partidoId);
}