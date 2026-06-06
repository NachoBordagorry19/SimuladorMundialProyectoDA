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
        partido.Local = _contexto.Equipos.First(e => e.Nombre == partido.Local.Nombre);
        partido.Visitante = _contexto.Equipos.First(e => e.Nombre == partido.Visitante.Nombre);
        partido.Estadio = _contexto.Estadios.First(e => e.Nombre == partido.Estadio.Nombre);

        _contexto.Partidos.Add(partido);
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

    public void ActualizarPartido(Partido partido)
    {
        _contexto.Partidos.Update(partido);
        _contexto.SaveChanges();
    }
}