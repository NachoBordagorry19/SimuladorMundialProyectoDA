namespace TestRepositorio;

public class PartidoRepositorioTest
{
    [TestMethod]
    public void ObtenerPartidos_CuandoNoHayPartidos_RetornaListaVacia()
    {
        var partidos = _partidoRepositorio.ObtenerPartidos();

        Assert.IsNotNull(partidos);
        Assert.AreEqual(0, partidos.Count);
    }
    
}