using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioNotificacion
{
    void GenerarNotificacionesParaPeriodistas(string mensaje);
    List<Notificacion> ObtenerNoLeidas(int usuarioId);
    void MarcarComoLeida(NotificacionDTO notDTO);
}