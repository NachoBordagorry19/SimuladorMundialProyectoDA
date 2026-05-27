namespace TestDominio;
using Dominio.Clases;
using Dominio.Enums;

[TestClass]
public class IncidenciaTest
{
    [TestMethod]
    public void CrearIncidencia_ConDatosValidos_DebeCrearseCorrectamente()
    {
        int partidoId = 1;
        int equipoId = 1;
        var tipo = TipoIncidencia.TarjetaAmarilla;

        var incidencia = new Incidencia { PartidoId = partidoId, EquipoId = equipoId, Tipo = tipo };

        Assert.AreEqual(partidoId, incidencia.PartidoId);
        Assert.AreEqual(equipoId, incidencia.EquipoId);
        Assert.AreEqual(tipo, incidencia.Tipo);
    }
}