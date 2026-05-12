using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

public class SessionServiceMock : ISessionService
{
    public UsuarioDTO ObtenerUsuarioLogeado()
    {
        return new UsuarioDTO { Email = "admin@worldcup.com" };
    }
}