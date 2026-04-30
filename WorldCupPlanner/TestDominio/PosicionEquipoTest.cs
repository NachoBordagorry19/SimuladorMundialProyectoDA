namespace TestDominio;

[TestClass]
public class PosicionEquipoTest
{
    [TestMethod]
    public void CrearPosicionEquipo_ValoresIniciales()
    {
        var equipo = new Equipo("Nacional", Confederacion.CONMEBOL, 10);
        var pos = new PosicionEquipo(equipo);

        Assert.AreEqual(0, pos.PartidosJugados);
        Assert.AreEqual(0, pos.Ganados);
        Assert.AreEqual(0, pos.Empatados);
        Assert.AreEqual(0, pos.Perdidos);
        Assert.AreEqual(0, pos.GolesAFavor);
        Assert.AreEqual(0, pos.GolesEnContra);
        Assert.AreEqual(0, pos.Puntos);
    }
}