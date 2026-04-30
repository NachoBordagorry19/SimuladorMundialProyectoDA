using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Clases;
using Dominio.Enums;

namespace TestDominio.Clases;

[TestClass]
public class PartidoTest
{
    [TestMethod]
    public void CrearPartido()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1100);
        Estadio estadio = new Estadio("Campeon del Siglo", "Montevideo", "Decano", 60000);

        Partido partido = new Partido(DateTime.Now, estadio, local, visitante, Fase.Grupos, 0, 0);

        Assert.AreEqual(estadio, partido.Estadio);
        Assert.AreEqual(local, partido.Local);
        Assert.AreEqual(visitante, partido.Visitante);
        Assert.AreEqual(Fase.Grupos, partido.Fase);
        Assert.AreEqual(EstadoPartido.Pendiente, partido.Estado);
        Assert.AreEqual(0, partido.GolesLocal);
        Assert.AreEqual(0, partido.GolesVisitante);
    }

    [TestMethod]
    public void Partido_IdEsIncremental()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1100);
        Estadio estadio = new Estadio("Campeon del Siglo", "Montevideo", "Decano", 60000);
        Grupo grupo = new Grupo("A");

        Partido p1 = new Partido(DateTime.Now, estadio, local, visitante, grupo);
        Partido p2 = new Partido(DateTime.Now, estadio, local, visitante, grupo);

        Assert.AreEqual(p1.Id + 1, p2.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Partido_FechaNoPuedeSerInvalida_TiroExcepcion()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1200);
        Estadio estadio = new Estadio("Estadio", "Ciudad", "Descripcion", 30000);
        Grupo grupo = new Grupo("A");

        new Partido(DateTime.MinValue, estadio, local, visitante, grupo);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Partido_SiEstadioEsNull_TiraExcepcion()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1200);
        Grupo grupo = new Grupo("A");

        new Partido(DateTime.Now, null, local, visitante, grupo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPartido_SiEquiposSonIguales_TiraExcepcion()
    {
        Equipo equipo = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Estadio estadio = new Estadio("Estadio", "Ciudad", "Desc", 30000);
        Grupo grupo = new Grupo("A");

        new Partido(DateTime.Now, estadio, equipo, equipo, grupo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPartido_SiGrupoEsNull_TiraExcepcion()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1200);
        Estadio estadio = new Estadio("Estadio", "Ciudad", "Desc", 30000);

        new Partido(DateTime.Now, estadio, local, visitante, null);
    }
}