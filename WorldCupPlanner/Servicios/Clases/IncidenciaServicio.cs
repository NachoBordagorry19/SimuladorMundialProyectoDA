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
        if (incidenciaDTO == null)
        {
            throw new ArgumentException("La incidencia no puede ser nula.");
        }

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

    public List<Incidencia> ObtenerIncidenciasPorPartido(int idPartido)
    {
        if (idPartido < 0)
        {
            throw new ArgumentException("El id de partido no debe ser menor a 0");
        }
        return _incidenciaRepositorio.ObtenerIncidencias(idPartido);
    }

    public void EliminarIncidencia(int idIncidencia)
    {
        if (idIncidencia < 0)
        {
            throw new ArgumentException("El id de incidencia no debe ser menor a 0");
        }
        var incidenciaExistente = _incidenciaRepositorio.ObtenerIncidenciaPorId(idIncidencia);
        if (incidenciaExistente == null)
        {
            throw new ArgumentException("La incidencia no existe.");
        }
        _incidenciaRepositorio.EliminarIncidencia(idIncidencia);
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