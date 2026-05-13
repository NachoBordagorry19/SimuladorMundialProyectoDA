using Servicios.Interfaces;
using Servicios.Modelo;

namespace UI.Estado;

public class SessionService : ISessionService
{
    private readonly UsuarioSesion _usuarioSesion;

    public SessionService(UsuarioSesion usuarioSesion)
    {
        _usuarioSesion = usuarioSesion;
    }

    public UsuarioDTO ObtenerUsuarioLogeado()
    {
        return _usuarioSesion.UsuarioActual ?? new UsuarioDTO { Email = string.Empty };
    }
}

