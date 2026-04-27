using System.Linq;
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

    public void AgregarEstadio(Estadio estadio)
    {
        var existe =  _BDEnMemoria.ObtenerEstadios().Any(e => e.Nombre.Equals(estadio.Nombre, StringComparison.OrdinalIgnoreCase));
        if (existe)
        {
            throw new InvalidOperationException("Ya existe un estadio con el nombre: " + estadio.Nombre);
        }
        _BDEnMemoria.AgregarEstadio(estadio);
    }

    public Estadio? ObtenerEstadioPorNombre(string nombre)
    {
        if (string.IsNullOrEmpty(nombre)) return null;
        return _BDEnMemoria.ObtenerEstadios().FirstOrDefault(e => e.Nombre == nombre);
    }

    public void EliminarEstadio(Estadio estadio)
    {
        _BDEnMemoria.BorrarEstadio(estadio);
    }
}