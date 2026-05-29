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
        if (incidenciaDTO.Id > 0)
        {
            var incidenciaExistente = _incidenciaRepositorio.ObtenerIncidenciaPorId(incidenciaDTO.Id);
            if (incidenciaExistente != null)
            {
                throw new ArgumentException("La incidencia con ese Id ya existe.");
            }
        }

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