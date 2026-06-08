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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}