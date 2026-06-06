using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class PartidoRepositorio : IPartidoRepositorio
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;

    public PartidoRepositorio(BaseDeDatosEnMemoria baseDeDatosEnMemoria)
    {
        _baseDeDatosEnMemoria = baseDeDatosEnMemoria;
    }

    public List<Partido> ObtenerPartidos()
    {
        return _baseDeDatosEnMemoria.ObtenerPartidos();
    }

    public void AgregarPartido(Partido partido)
    {
        partido.Id = _baseDeDatosEnMemoria.ObtenerPartidos().Count + 1;
        _baseDeDatosEnMemoria.AgregarPartido(partido);
    }

    public Partido? ObtenerPartidoPorId(int id)
    {
        return _baseDeDatosEnMemoria.ObtenerPartidos().FirstOrDefault(p => p.Id == id);
    }

    public void EliminarPartido(Partido partido)
    {
        _baseDeDatosEnMemoria.BorrarPartido(partido);
    }

    public void ActualizarPartido(Partido partido)
    {
        _baseDeDatosEnMemoria.ActualizarPartido(partido);
    }
}