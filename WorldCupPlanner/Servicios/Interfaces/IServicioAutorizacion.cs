using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioAutorizacion
{
    bool PuedeGestionarUsuarios(UsuarioDTO? usuario);
    bool PuedeGestionarEquipos(UsuarioDTO? usuario);
    bool PuedeGestionarEstadios(UsuarioDTO? usuario);
    bool PuedeVisualizarLogs(UsuarioDTO? usuario);

    bool PuedeUsarFixture(UsuarioDTO? usuario);
    bool PuedeEditarPartidos(UsuarioDTO? usuario);
    bool PuedeImportar(UsuarioDTO? usuario);

    bool PuedeEliminarUsuario(UsuarioDTO? usuarioActual, string emailAEliminar);
}