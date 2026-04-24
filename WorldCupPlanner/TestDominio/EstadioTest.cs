using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class EstadioTest
{
    [TestMethod]
    public void Estadio()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", "El mejor estadio del mundo", 75024);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_SinNombre_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("", "Munich", "El mejor estadio del mundo", 75024);
    }
}