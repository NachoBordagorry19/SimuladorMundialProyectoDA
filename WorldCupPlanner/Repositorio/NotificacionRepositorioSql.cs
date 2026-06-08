using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class NotificacionRepositorioSql : INotificacionRepositorio
{
    private SqlContexto _contexto;

    public NotificacionRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }
    public void AgregarNotificacion(Notificacion not)
    {
        _contexto.Notificaciones.Add(not);
        _contexto.SaveChanges();
    }

    public void MarcarComoLeida(int notId)
    {
        throw new NotImplementedException();
    }

    public List<Notificacion> ObtenerNotificaciones(int usuarioId)
    {
        return _contexto.Notificaciones.ToList();
    }

    public List<Notificacion> ObtenerNotificacionesNoLeidas(int usuarioId)
    {
        List<Notificacion> noLeidas = new List<Notificacion>();
        List<Notificacion> notificaciones = _contexto.Notificaciones.ToList();
        foreach (var not in notificaciones)
        {
            if (not.Leida == false && not.UsuarioId == usuarioId)
            {
                noLeidas.Add(not);
            }
        }
        return noLeidas;
    }
}