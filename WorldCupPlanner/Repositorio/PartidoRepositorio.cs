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
}