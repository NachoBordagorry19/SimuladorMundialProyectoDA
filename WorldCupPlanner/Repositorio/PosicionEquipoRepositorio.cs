using Repositorio.Interfaces;
using System.Linq;
using Dominio.Clases;

namespace Repositorio;

public class PosicionEquipoRepositorio : IPosicionEquipoRepositorio
{
    private BaseDeDatosEnMemoria _BDEnMemoria;

    public PosicionEquipoRepositorio(BaseDeDatosEnMemoria BDEnMemoria)
    {
        _BDEnMemoria = BDEnMemoria;
    }
}