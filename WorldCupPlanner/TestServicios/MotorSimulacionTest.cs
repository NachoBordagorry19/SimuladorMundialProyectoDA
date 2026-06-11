using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Clases;
using Dominio.Enums;
using Servicios.Clases;

namespace TestServicios;

[TestClass]
public class MotorSimulacionTest
{
    private Partido CrearPartidoConRankings(int rankingLocal, int rankingVisitante)
    {
        var estadio = new Estadio("Estadio", "Ciudad", "Descripcion", 50000);
        var local = new Equipo("LocalFC", Confederacion.UEFA, rankingLocal);
        var visitante = new Equipo("VisitanteFC", Confederacion.CONMEBOL, rankingVisitante);
        return new Partido(DateTime.Now, estadio, local, visitante, Fase.Grupos, 0, 0);
    }

    [TestMethod]
    public void MotorProbabilistico_Nombre_EsElCorrecto()
    {
        var motor = new MotorProbabilistico();

        Assert.AreEqual("Probabilístico", motor.Nombre);
    }

    [TestMethod]
    public void MotorProbabilistico_Simular_ConLocalMuySuperior_GolesLocalSonDelGanador()
    {
        var motor = new MotorProbabilistico();
        var partido = CrearPartidoConRankings(rankingLocal: 2500, rankingVisitante: 300);
        var random = new Random(42);

        var (golesLocal, golesVisitante) = motor.Simular(partido, random);

        Assert.IsTrue(golesLocal >= 1 && golesLocal <= 3);
        Assert.IsTrue(golesVisitante >= 0 && golesVisitante <= 1);
    }
}