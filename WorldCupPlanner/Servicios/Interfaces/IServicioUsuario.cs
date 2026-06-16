using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioUsuario
{
    public void AgregarUsuario(UsuarioDTO UsuarioDto);
    public List<UsuarioDTO> ObtenerUsuarios();
    public UsuarioDTO ObtenerUsuario(string email);
    public void EliminarUsuario(UsuarioDTO usuarioDto);
    public List<UsuarioDTO> ObtenerUsuariosEliminables(string emailUsuarioActual);
    public void EliminarUsuario(string emailSeleccionado, string emailConfirmado, string emailUsuarioActual);
    public UsuarioDTO AutenticarUsuario(string email, string contrasena);
    public void ActualizarUsuario(UsuarioDTO usuarioDto);
    public void ActualizarUsuario(string emailOriginal, UsuarioDTO usuarioDto);
    public void ReiniciarContrasena(string email);
}