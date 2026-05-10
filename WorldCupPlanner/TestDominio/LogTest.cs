using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class LogTest
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validar_CuandoMensajeEsVacio_LanzaExcepcion()
    {
        Log log = new Log();
        log.Mensaje = "";
        log.Usuario = "admin@worldcup.com";

        log.Validar();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validar_CuandoUsuarioEsNulo_LanzaExcepcion()
    {
        Log log = new Log();
        log.Mensaje = "Mensaje";
        log.Usuario = "";
        
        log.Validar();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validar_CuandoMensajeSonSoloEspacios_LanzaExcepcion()
    {
        Log log = new Log();
        log.Mensaje = "   ";
        log.Usuario = "admin@worldcup.com";

        log.Validar();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Validar_CuandoUsuarioSonSoloEspacios_LanzaExcepcion()
    {
        Log log = new Log();
        log.Mensaje = "Usuario logueado";
        log.Usuario = "   ";

        log.Validar();
    }
    
    [TestMethod]
    public void Validar_ConDatosCorrectos_NoLanzaExcepcion()
    {
        Log log = new Log { 
            Mensaje = "Usuario logueado", 
            Usuario = "admin@cup.com" 
        };

        log.Validar();
        Assert.IsNotNull(log.Mensaje);
        Assert.AreEqual("admin@cup.com", log.Usuario);
    }
}