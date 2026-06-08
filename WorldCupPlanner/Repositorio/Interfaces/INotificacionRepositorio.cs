using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface INotificacionRepositorio
{
        void AgregarNotificacion(Notificacion not);
        void MarcarComoLeida(int notId);
        List<Notificacion> ObtenerNotificaciones(int usuarioId);
        List<Notificacion> ObtenerNotificacionesNoLeidas(int usuarioId);
}