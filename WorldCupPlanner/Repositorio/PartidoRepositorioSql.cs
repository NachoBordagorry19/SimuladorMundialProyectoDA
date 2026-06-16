using Dominio.Clases;
using Microsoft.EntityFrameworkCore;
using Repositorio.Interfaces;

namespace Repositorio;

public class PartidoRepositorioSql : IPartidoRepositorio
{
    private SqlContexto _contexto;

    public PartidoRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Partido> ObtenerPartidos()
    {
        return _contexto.Partidos
            .Include(p => p.Local)
            .Include(p => p.Visitante)
            .Include(p => p.Estadio)
            .ToList();
    }

    public void AgregarPartido(Partido partido)
    {
        var local = _contexto.Equipos.FirstOrDefault(e => e.Nombre == partido.Local.Nombre);
        if (local != null) partido.Local = local;

        var visitante = _contexto.Equipos.FirstOrDefault(e => e.Nombre == partido.Visitante.Nombre);
        if (visitante != null) partido.Visitante = visitante;

        var estadio = _contexto.Estadios.FirstOrDefault(e => e.Nombre == partido.Estadio.Nombre);
        if (estadio != null) partido.Estadio = estadio;

        _contexto.Partidos.Add(partido);
        _contexto.SaveChanges();
    }

    public void ActualizarPartido(Partido partido)
    {
        if (partido.Estadio.Id == 0)
        {
            var estadio = _contexto.Estadios.FirstOrDefault(e => e.Nombre == partido.Estadio.Nombre);
            if (estadio != null) partido.Estadio = estadio;
        }
        _contexto.SaveChanges();
    }

    public Partido? ObtenerPartidoPorId(int id)
    {
        return _contexto.Partidos
            .Include(p => p.Local)
            .Include(p => p.Visitante)
            .Include(p => p.Estadio)
            .FirstOrDefault(p => p.Id == id);
    }

    public void EliminarPartido(Partido partido)
    {
        _contexto.Partidos.Remove(partido);
        _contexto.SaveChanges();
    }
}