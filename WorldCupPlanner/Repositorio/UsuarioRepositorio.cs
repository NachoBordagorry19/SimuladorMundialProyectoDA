using Dominio.Clases;

namespace Repositorio;

public class UsuarioRepositorio
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;

    public UsuarioRepositorio(BaseDeDatosEnMemoria baseDeDatosEnMemoria)
    {
        _baseDeDatosEnMemoria = baseDeDatosEnMemoria;
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _baseDeDatosEnMemoria.ObtenerUsuarios();
    }
}