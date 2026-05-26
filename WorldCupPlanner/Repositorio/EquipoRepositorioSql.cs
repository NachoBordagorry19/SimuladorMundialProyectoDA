using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class EquipoRepositorioSql:IEquipoRepositorio
{
    private SqlContexto _contexto;

    public EquipoRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Equipo> ObtenerEquipos()
    {
        return _contexto.Equipos.ToList();
    }

    public void AgregarEquipo(Equipo equipo)
    {
        _contexto.Equipos.Add(equipo);
        _contexto.SaveChanges();
    }

    public void EliminarEquipo(Equipo equipo)
    {
        _contexto.Equipos.Remove(equipo);
        _contexto.SaveChanges();
    }

    public Equipo ObtenerEquipo(Func<Equipo, bool> filtro)
    {
        return _contexto.Equipos.ToList().Where(filtro).FirstOrDefault();
    }

    public void ActualizarEquipo(Equipo equipo)
    {
        _contexto.Equipos.Update(equipo);
        _contexto.SaveChanges();
    }
}