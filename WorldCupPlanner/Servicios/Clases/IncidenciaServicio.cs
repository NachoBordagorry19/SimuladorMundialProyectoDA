using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class IncidenciaServicio
{
    private readonly IIncidenciaRepositorio _incidenciaRepositorio;

    public IncidenciaServicio(IIncidenciaRepositorio incidenciaRepositorio)
    {
        _incidenciaRepositorio =  incidenciaRepositorio;
    }

    public void AgregarIncidencia(IncidenciaDTO incidenciaDTO)
    {
        Incidencia incidencia = IncidenciaDTOAEntidad(incidenciaDTO);
        _incidenciaRepositorio.AgregarIncidencia(incidencia);
    }

    public Incidencia IncidenciaDTOAEntidad(IncidenciaDTO incidenciaDTO)
    {
        var Incidencia = new Incidencia(
            incidenciaDTO.IdEquipo,
            incidenciaDTO.IdPartido, 
            incidenciaDTO.TipoIncidencia
            );
        return Incidencia;
    }
}