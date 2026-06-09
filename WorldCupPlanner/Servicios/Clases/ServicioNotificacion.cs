using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioNotificacion : IServicioNotificacion
{
    private readonly INotificacionRepositorio _notificacionRepositorio;
    private readonly IServicioAuditoria _auditoriaRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    public ServicioNotificacion(INotificacionRepositorio notificacionRepositorio, IUsuarioRepositorio usuarioRepositorio, IServicioAuditoria auditoria)
    {
        _notificacionRepositorio = notificacionRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _auditoriaRepositorio = auditoria;
    }


    public void GenerarNotificaciones(string mensaje)
    {
        throw new NotImplementedException();
    }

    public List<NotificacionDTO> ObtenerNoLeidas(int usuarioId)
    {
        throw new NotImplementedException();
    }

    public void MarcarComoLeida(NotificacionDTO notDTO)
    {
        throw new NotImplementedException();
    }
}