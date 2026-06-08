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

    public bool PuedeUsarFixture(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Editor);
    }

    public bool PuedeEditarPartidos(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Editor);
    }

    public bool PuedeImportar(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Editor);
    }

    public bool PuedeEliminarUsuario(UsuarioDTO usuarioActual, string emailAEliminar)
    {
        return usuarioActual.Roles.Contains(Rol.Administrador)
               && usuarioActual.Email != emailAEliminar;
    }
    
    public bool PuedeVerFixture(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Editor) || usuario.Roles.Contains(Rol.Periodista);
    }

    public bool PuedeVerEstadisticas(UsuarioDTO usuario)
    {
        return usuario.Roles.Contains(Rol.Administrador) || usuario.Roles.Contains(Rol.Periodista);
    }
}