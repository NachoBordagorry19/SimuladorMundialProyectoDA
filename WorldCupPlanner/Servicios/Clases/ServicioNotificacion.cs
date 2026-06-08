using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioNotificacion : IServicioNotificacion
{
    private readonly INotificacionRepositorio _notificacionRepositorio;
    private readonly IServicioAuditoria _auditoriaRepositorio;

    public ServicioNotificacion(INotificacionRepositorio notificacionRepositorio, IServicioAuditoria auditoria)
    {
        _notificacionRepositorio = notificacionRepositorio;
        _auditoriaRepositorio = auditoria;
    }


    public void GenerarNotificaciones(string mensaje)
    {
        throw new NotImplementedException();
    }

    public List<Notificacion> ObtenerNoLeidas(int usuarioId)
    {
        throw new NotImplementedException();
    }

    public void MarcarComoLeida(NotificacionDTO notDTO)
    {
        throw new NotImplementedException();
    }
}