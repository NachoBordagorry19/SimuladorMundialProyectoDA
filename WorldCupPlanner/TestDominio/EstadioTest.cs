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

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_MayorA80Caracteres_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("Nombre con longitud excesiva para pruebas unitarias del sistema (81 caracteres)!!", "Munich", "El mejor estadio del mundo", 75024);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_SinCiudad_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "", "El mejor estadio del mundo", 75024);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_ConCiudadMayorA60Caracteres_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "CiudadPruebaConExactamenteSesentaYUnCaracteres12345ABCDEFGHIJ", "El mejor estadio del mundo", 75024);
    }

    [TestMethod]
    public void CreoEstadio_ConDescripcionVacia_YSeCrea()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", null, 75024);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_ConDescripcionMayorA400Caracteres_LanzoExcepcion()
    {
        var descripcion401 = new string('a', 401);
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", descripcion401, 75024);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_ConCapacidadLocativaVacia_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", "Desc", null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoEstadio_ConCapacidadMenorA0_LanzoExcepcion()
    {
        Estadio estadioPrueba = new Estadio("Allianz Arena", "Munich", "Desc", -5);
    }
}