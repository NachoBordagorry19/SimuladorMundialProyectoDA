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

    public void AgregarUsuario(Usuario usuario)
    {
        _baseDeDatosEnMemoria.AgregarUsuario(usuario);
    }

    public Usuario? ObtenerUsuario(Func<Usuario, bool> filter)
    {
        return _baseDeDatosEnMemoria.ObtenerUsuarios().Where(filter).FirstOrDefault();
    }
}