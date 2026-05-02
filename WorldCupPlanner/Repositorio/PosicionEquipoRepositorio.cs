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

    public List<PosicionEquipo> ObtenerPosiciones()
    {
        throw new NotImplementedException();
    }

    public void AgregarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }

    public PosicionEquipo? ObtenerPosicionPorEquipo(string nombreEquipo)
    {
        throw new NotImplementedException();
    }

    public bool ActualizarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }

    public void EliminarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }
}