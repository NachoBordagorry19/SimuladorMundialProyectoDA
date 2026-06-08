using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioNotificacion
{
    void GenerarNotificaciones(string mensaje);
    List<NotificacionDTO> ObtenerNoLeidas(int usuarioId);
    void MarcarComoLeida(NotificacionDTO notDTO);
}