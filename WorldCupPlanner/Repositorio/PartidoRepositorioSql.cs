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
        _contexto.Partidos.Add(partido);
        _contexto.SaveChanges();
    }

    public Partido? ObtenerPartidoPorId(int id)
    {
        throw new NotImplementedException();
    }

    public void EliminarPartido(Partido partido)
    {
        throw new NotImplementedException();
    }

    public void ActualizarPartido(Partido partido)
    {
        throw new NotImplementedException();
    }
}