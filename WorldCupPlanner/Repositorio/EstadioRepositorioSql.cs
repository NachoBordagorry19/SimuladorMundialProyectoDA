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

    public void AgregarEstadio(Estadio estadio)
    {
        _contexto.Estadios.Add(estadio);
        _contexto.SaveChanges();
    }

    public void EliminarEstadio(Estadio estadio)
    {
        _contexto.Estadios.Remove(estadio);
        _contexto.SaveChanges();
    }

    public Estadio ObtenerEstadio(Func<Estadio, bool> filtro)
    {
        return _contexto.Estadios.ToList().Where(filtro).FirstOrDefault();
    }
}