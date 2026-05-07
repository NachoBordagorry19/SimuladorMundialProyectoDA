using Dominio.Clases;
using Dominio.Enums;

namespace TestDominio;

[TestClass]
public class EquipoTest
{
    [TestMethod]
    public void CrearEquipo()
    {
        Equipo equipoPrueba = new Equipo("Nacional", Confederacion.UEFA, 400);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinNombre_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("", Confederacion.UEFA, 400);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_NombreMayorA60Caracteres_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Esta cadena tiene exactamente sesenta y un caracteres ahora!!",
            Confederacion.UEFA, 400);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_SinRankingFifa_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Nacional", Confederacion.UEFA, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEquipo_ConRankingFifaNegativo_LanzoExcepcion()
    {
        Equipo equipoPrueba = new Equipo("Nacional", Confederacion.UEFA, -50);
    }

    [TestMethod]
    public void CrearEquipo_SeCreaConPuntos_Goles_DiferenciaDeGoles_EnCero()
    {
        Equipo equipo = new Equipo("Nacional", Confederacion.CONMEBOL, 20);
        Assert.AreEqual(0,equipo.Puntos);
        Assert.AreEqual(0,equipo.GolesAFavor);
        Assert.AreEqual(0,equipo.DiferenciaDeGoles);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SeQuiereAgregarPuntosNegativos_LanzoExcepcion()
    {
        Equipo equipo = new Equipo("Nacional", Confederacion.CONMEBOL, 20);
        int puntos = -20;
        equipo.Puntos = puntos;
    }
}