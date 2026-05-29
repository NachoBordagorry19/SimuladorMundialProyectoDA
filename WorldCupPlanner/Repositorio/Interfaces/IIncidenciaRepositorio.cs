using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IIncidenciaRepositorio
{
    public List<Incidencia> ObtenerIncidencias(int idPartido);
    public void AgregarIncidencia(Incidencia incidencia);
    public void EliminarIncidencia(int  idIncidencia);
}