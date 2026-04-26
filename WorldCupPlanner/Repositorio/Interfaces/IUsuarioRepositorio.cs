using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IUsuarioRepositorio
{
    List<Usuario> ObtenerUsuarios();
    void AgregarUsuario(Usuario usuario);
    Usuario? ObtenerUsuario(Func<Usuario,bool> filtro);
    void ActualizarUsuario(Usuario usuario);
    void EliminarUsuario(Usuario usuario);
}