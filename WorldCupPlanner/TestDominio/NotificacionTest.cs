using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class NotificacionTest
{
    [TestMethod]
    public void CrearNotificacion()
    {
        DateTime fechaHora = DateTime.Now;
        Usuario usuario = new Usuario();
        Notificacion not = new Notificacion("Mensaje", fechaHora, usuario.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearNotificacion_SinMensaje_LanzaExcepcion()
    {
        DateTime fechaHora = DateTime.Now;
        Usuario usuario = new Usuario();
        Notificacion not = new Notificacion("", fechaHora, usuario.Id);
    }

    [TestMethod]
    public void MarcarComoLeida_MarcaComoLeida()
    {
        DateTime fechaHora = DateTime.Now;
        Usuario usuario = new Usuario();
        Notificacion not = new Notificacion("Mensaje", fechaHora, usuario.Id);
        not.MarcarComoLeida();
        Assert.IsTrue(not.YaLeida());
    }
}