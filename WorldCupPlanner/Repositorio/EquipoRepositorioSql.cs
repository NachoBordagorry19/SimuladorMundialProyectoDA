using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class EquipoRepositorioSql
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
}