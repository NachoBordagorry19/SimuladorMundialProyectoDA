using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class AuditoriaTest
{
    [TestMethod]
    public void CrearAuditoria_AsignaPropiedadesCorrectamente()
    {
        DateTime ahora = DateTime.Now;

        var auditoria = new Auditoria
        {
            Usuario = "admin@cup.com",
            Accion = "Login",
            Detalle = "Se logueó correctamente",
            FechaHora = ahora
        };

        Assert.AreEqual("admin@cup.com", auditoria.Usuario);
        Assert.AreEqual("Login", auditoria.Accion);
        Assert.AreEqual("Se logueó correctamente", auditoria.Detalle);
        Assert.AreEqual(ahora, auditoria.FechaHora);
    }
}