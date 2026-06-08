using Dominio.Clases;

namespace Servicios.Interfaces;

public interface IServicioNotificacion
{
    void GenerarNotificacionesParaPeriodistas(string mensaje);
    List<Notificacion> ObtenerNoLeidas(int usuarioId);
    void MarcarComoLeida(int notId);
}