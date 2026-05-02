using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IPartidoRepositorio
{
    List<Partido> ObtenerPartidos();
    void AgregarPartido(Partido partido);
    Partido? ObtenerPartidoPorId(int id);
    void EliminarPartido(Partido partido);
    bool ActualizarPartido(Partido partido);
}