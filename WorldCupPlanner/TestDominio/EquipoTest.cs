using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class EquipoTest
{
    [TestMethod]
    public void CrearEquipo()
    {
        Equipo equipoPrueba = new Equipo("Nacional", "UEFA", "400");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinNombre_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("", "UEFA", "400");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_NombreMayorA60Caracteres_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Esta cadena tiene exactamente sesenta y un caracteres ahora!!", "UEFA", "400");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinConfederacion_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Nacional", "", "400");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_ConConfederacionDesconocida_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Nacional", "Australiana", "400");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinRankingFifa_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Nacional", "UEFA", "");
    }
}