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

        var incidencia = new Incidencia(partidoId, equipoId, tipo);

        Assert.AreEqual(partidoId, incidencia._idPartido);
        Assert.AreEqual(equipoId, incidencia._idEquipo);
        Assert.AreEqual(tipo, incidencia._tipoIncidencia);
    }
}