using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class PartidoRepositorio : IPartidoRepositorio
{
    private BaseDeDatosEnMemoria _BDEnMemoria;

    public PartidoRepositorio(BaseDeDatosEnMemoria BDEnMemoria)
    {
        _BDEnMemoria = BDEnMemoria;
    }

    public List<Partido> ObtenerPartidos()
    {
        return _BDEnMemoria.ObtenerPartidos();
    }

    public void AgregarPartido(Partido partido)
    {
        _BDEnMemoria.AgregarPartido(partido);
    }

    public Partido? ObtenerPartidoPorId(int id)
    {
        return _BDEnMemoria.ObtenerPartidos().FirstOrDefault(p => p.Id == id);
    }

    public void EliminarPartido(Partido partido)
    {
        _BDEnMemoria.BorrarPartido(partido);
    }

    public void ActualizarPartido(Partido partido)
    {
        _BDEnMemoria.ActualizarPartido(partido);
    }
}