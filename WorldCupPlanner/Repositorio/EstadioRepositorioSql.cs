using Dominio.Clases;

namespace Repositorio;

public class EstadioRepositorioSql
{
    private SqlContexto _contexto;

    public EstadioRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Estadio> ObtenerEstadios()
    {
        return _contexto.Estadios.ToList();
    }
}