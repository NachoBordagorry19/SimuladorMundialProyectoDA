using NUnit.Framework;
using Dominio.Clases;
using Dominio.Enums;

namespace TestDominio.Clases;

[TestFixture]
public class TestsPartido
{
    [Test]
    public void RegistrarResultado_DeberiaAsignarGolesYEstadoJugado()
    {
        var equipo1 = new Equipo("Uruguay", Confederacion.CONMEBOL, 1);
        var equipo2 = new Equipo("Brasil", Confederacion.CONMEBOL, 2);
        var estadio = new Estadio("Azteca", "Ciudad de Mexico", 83000);

        var partido = new Partido(equipo1, equipo2, estadio);

        partido.RegistrarResultado(2, 1);

        Assert.AreEqual(2, partido.GolesLocal);
        Assert.AreEqual(1, partido.GolesVisitante);
        Assert.AreEqual(EstadoPartido.Jugado, partido.Estado);
    }
}