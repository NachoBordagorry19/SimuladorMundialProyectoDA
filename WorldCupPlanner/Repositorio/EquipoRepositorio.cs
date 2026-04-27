using Dominio.Clases;

namespace Repositorio;

public class EquipoRepositorio
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;

    public EquipoRepositorio(BaseDeDatosEnMemoria baseDeDatosEnMemoria)
    {
        _baseDeDatosEnMemoria = baseDeDatosEnMemoria;
    }

    public List<Equipo> ObtenerEquipos()
    {
        return _baseDeDatosEnMemoria.ObtenerEquipos();
    }
}