using Dominio.Clases;
using Dominio.Enums;
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
        List<Usuario> usuarios = _usuarioRepositorio.ObtenerUsuarios();
        foreach (var usuario in usuarios)
        {
            if (usuario.Roles.Contains(Rol.Periodista))
            {
                Notificacion notificacion = new Notificacion
                {
                    Mensaje = mensaje,
                    FechaHora = DateTime.Now,
                    UsuarioId = usuario.Id,
                    Leida = false
                };
                _notificacionRepositorio.AgregarNotificacion(notificacion);
            }
        }
    }
//_auditoriaRepositorio.RegistrarGeneracionNotificacion(usuario.Nombre, mensaje);
// interfaz

    public List<NotificacionDTO> ObtenerNoLeidas(int usuarioId)
    {
        List<Notificacion> notsNoLeidas = _notificacionRepositorio.ObtenerNotificacionesNoLeidas(usuarioId);
        List<NotificacionDTO> notsNoLeidasDTO = new List<NotificacionDTO>();
        foreach (var not in notsNoLeidas)
        {
            notsNoLeidasDTO.Add(NotificacionEntidadADTO(not));
        }

        return notsNoLeidasDTO;
    }

    public void MarcarComoLeida(NotificacionDTO notDTO)
    {
        Notificacion not = NotificacionDTOAEntidad(notDTO);
        _notificacionRepositorio.MarcarComoLeida(not);
        notDTO.Leida = true;
    }
    
    public Notificacion NotificacionDTOAEntidad(NotificacionDTO notificacionDto)
    {
        var notificacion = new Notificacion(
            notificacionDto.Mensaje,
            notificacionDto.FechaHora,
            notificacionDto.UsuarioId
        );
        notificacion.Id = notificacionDto.Id;
        notificacion.Leida = notificacionDto.Leida;
        return notificacion;
    }
    
    public NotificacionDTO NotificacionEntidadADTO(Notificacion notificacion)
    {
        return new NotificacionDTO()
        {
            Id = notificacion.Id,
            Mensaje = notificacion.Mensaje,
            FechaHora = notificacion.FechaHora,
            UsuarioId = notificacion.UsuarioId,
            Leida = notificacion.Leida
        };
    }
}