using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface ISessionService
{
    UsuarioDTO ObtenerUsuarioLogeado();
}