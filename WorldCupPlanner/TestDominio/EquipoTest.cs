using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class EquipoTest
{
    [TestMethod]
    public void CrearEquipo()
    {
        Equipo equipoPrueba = new Equipo("Nacional", "UEFA", 400);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinNombre_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("", "UEFA", 400);
    }
}