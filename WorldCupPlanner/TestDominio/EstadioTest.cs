namespace TestDominio;

[TestClass]
public class EstadioTest
{
    [TestMethod]
    public void Estadio()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", "El mejor estadio del mundo", 75.024);
    }
}