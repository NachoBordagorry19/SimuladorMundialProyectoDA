using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class AuditoriaRepositorioSql : IAuditoriaRepositorio
{
    private SqlContexto _contexto;

    public AuditoriaRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }
    public void AgregarRegistro(Auditoria registro)
    {
        _contexto.Auditorias.Add(registro);
        _contexto.SaveChanges();
    }

    public List<Auditoria> ObtenerTodosLosRegistros()
    {
        return _contexto.Auditorias
            .OrderByDescending(a => a.FechaHora)
            .ToList();
    }

    public List<Auditoria> ObtenerPorRango(DateTime desde, DateTime hasta)
    {
        return _contexto.Auditorias
            .Where(a => a.FechaHora >= desde && a.FechaHora <= hasta)
            .OrderByDescending(a => a.FechaHora)
            .ToList();
    }
}