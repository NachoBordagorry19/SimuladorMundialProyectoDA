using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class NotificacionRepositorioSqlTest
{
    private NotificacionRepositorioSql _notificacionRepositorioSql;
    private Notificacion _notificacion;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void Inicializar()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _notificacionRepositorioSql = new NotificacionRepositorioSql(_contexto);
        DateTime fechaHora = DateTime.Now;
        Usuario usuario = new Usuario();
        _notificacion = new Notificacion("Mensaje", fechaHora, usuario.Id);
    }

    [TestMethod]
    public void ObtenerNotificaciones_SeObtienenCorrectamente()
    {
        List<Notificacion> notificaciones = _notificacionRepositorioSql.ObtenerNotificaciones(1);
        Assert.AreEqual(0, notificaciones.Count);
    }

    [TestMethod]
    public void AgregarNotificacion_SeAgregaCorrectamente()
    {
        _notificacionRepositorioSql.AgregarNotificacion(_notificacion);
        List<Notificacion> notificaciones = _notificacionRepositorioSql.ObtenerNotificaciones(1);
        Assert.AreEqual(1, notificaciones.Count);
    }
    
    [TestMethod]
    public void ObtenerNotificacionesNoLeidas_SeObtienenCorrectamente()
    {
        Notificacion not = new Notificacion("Mensaje", DateTime.Now, 1);
        not.MarcarComoLeida();
        _notificacionRepositorioSql.AgregarNotificacion(not);
        _notificacionRepositorioSql.AgregarNotificacion(_notificacion);
        List<Notificacion> notificaciones = _notificacionRepositorioSql.ObtenerNotificacionesNoLeidas(1);
        
        Assert.AreEqual(1, notificaciones.Count);
    }
}