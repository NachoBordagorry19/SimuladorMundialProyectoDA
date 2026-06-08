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
}