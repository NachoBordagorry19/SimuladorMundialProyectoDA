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
        var local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        var visitante = new Equipo("B", Confederacion.UEFA, 1100);
        var estadio = new Estadio("Campeon del Siglo", "Montevideo", "Decano", 60000);
        var grupo = new Grupo("A");

        Partido partido = new Partido(1, DateTime.Now, estadio, local, visitante, grupo);
    }
}