using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface INotificacionRepositorio
{
        void AgregarNotificacion(Notificacion not);
        void MarcarComoLeida(Notificacion not);
        List<Notificacion> ObtenerNotificaciones(int usuarioId);
        List<Notificacion> ObtenerNotificacionesNoLeidas(int usuarioId);
}