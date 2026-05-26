using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class EstadioRepositorioSql:IEstadioRepositorio
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

    public Estadio ObtenerEstadioPorNombre(string nombre)
    {
        return _contexto.Estadios.FirstOrDefault(e => e.Nombre == nombre);
    }

    public bool ActualizarEstadio(Estadio estadio)
    {
        _contexto.Estadios.Update(estadio);
        _contexto.SaveChanges();
        return true;
    }
}