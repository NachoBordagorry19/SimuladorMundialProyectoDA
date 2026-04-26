using Dominio.Clases;
namespace Repositorio;

public class EstadioRepositorio
{
    private BaseDeDatosEnMemoria _BDEnMemoria;

    public EstadioRepositorio(BaseDeDatosEnMemoria BDEnMemoria)
    {
        _BDEnMemoria = BDEnMemoria;
    }

    public List<Estadio> ObtenerEstadios()
    {
        return _BDEnMemoria.ObtenerEstadios();
    }
}