using Dominio.Enums;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioAutorizacion : IServicioAutorizacion
{
    public bool PuedeGestionarUsuarios(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Administrador);
    }

    public bool PuedeGestionarEquipos(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Administrador);
    }

    public bool PuedeGestionarEstadios(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Administrador);
    }

    public bool PuedeVisualizarLogs(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Administrador);
    }

    public bool PuedeUsarFixture(UsuarioDTO? usuario)
    {
        throw new NotImplementedException();
    }

    public bool PuedeEditarPartidos(UsuarioDTO? usuario)
    {
        throw new NotImplementedException();
    }

    public bool PuedeImportar(UsuarioDTO? usuario)
    {
        throw new NotImplementedException();
    }

    public bool PuedeEliminarUsuario(UsuarioDTO? usuarioActual, string emailAEliminar)
    {
        throw new NotImplementedException();
    }
}