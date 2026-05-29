using Dominio.Clases;
using Microsoft.Data.SqlClient;

namespace Repositorio;

public class IncidenciaRepositorio
{
    private SqlContexto _contexto;

    public IncidenciaRepositorio(SqlContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Incidencia> ObtenerIncidencias(int idPartido)
    {
        return _contexto.Incidencias.Where(i => i._idPartido == idPartido).ToList();
    }

    public void AgregarIncidencia(Incidencia incidencia)
    {
        _contexto.Incidencias.Add(incidencia);
        _contexto.SaveChanges();
    }

    public void EliminarIncidencia(int id)
    {
        var incidencia = _contexto.Incidencias.Find(id);
        _contexto.Incidencias.Remove(incidencia);
        _contexto.SaveChanges();
    }
}