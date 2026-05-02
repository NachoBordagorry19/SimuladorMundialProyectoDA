using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IPartidoRepositorio
{
    List<Partido> ObtenerPartidos();
    void AgregarPartido(Partido partido);
}