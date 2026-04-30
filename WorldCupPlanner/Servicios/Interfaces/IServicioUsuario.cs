using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioUsuario
{
    public void AgregarUsuario(UsuarioDTO UsuarioDto);
    public List<UsuarioDTO> ObtenerUsuarios();
    public UsuarioDTO ObtenerUsuario(string email);
    public void EliminarUsuario(UsuarioDTO usuarioDto);

}