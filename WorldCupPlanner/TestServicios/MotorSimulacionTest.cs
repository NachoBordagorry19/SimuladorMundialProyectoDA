using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Enums;
using Servicios.Clases;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class MotorSimulacionTest
{
    private PartidoDTO CrearPartidoDTO(int rankingLocal, int rankingVisitante)
    {
        return new PartidoDTO
        {
            EquipoLocal = new EquipoDTO { Nombre = "LocalFC", RankingFifa = rankingLocal, Confederacion = Confederacion.UEFA },
            EquipoVisitante = new EquipoDTO { Nombre = "VisitanteFC", RankingFifa = rankingVisitante, Confederacion = Confederacion.CONMEBOL },
            Fecha = DateTime.Now,
            Estadio = new EstadioDTO { Nombre = "Estadio", Ciudad = "Ciudad", Descripcion = "Desc", CapacidadLocativa = 50000 },
            Fase = Fase.Grupos,
            EstadoPartido = EstadoPartido.Pendiente
        };
    }

    [TestMethod]
    public void MotorAleatorioPuro_Nombre_EsElCorrecto()
    {
        var motor = new MotorAleatorioPuro();

        Assert.AreEqual("Aleatorio Puro", motor.Nombre);
    }

    [TestMethod]
    public void MotorAleatorioPuro_Simular_IgnoraElRanking()
    {
        var motor = new MotorAleatorioPuro();
        var partidoConLocalSuperior = CrearPartidoDTO(rankingLocal: 2500, rankingVisitante: 300);
        var partidoConVisitanteSuperior = CrearPartidoDTO(rankingLocal: 300, rankingVisitante: 2500);

        motor.Simular(partidoConLocalSuperior, new Random(42));
        motor.Simular(partidoConVisitanteSuperior, new Random(42));

        Assert.AreEqual(partidoConLocalSuperior.GolesLocal, partidoConVisitanteSuperior.GolesLocal);
        Assert.AreEqual(partidoConLocalSuperior.GolesVisitante, partidoConVisitanteSuperior.GolesVisitante);
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
        var partidoDto = CrearPartidoDTO(rankingLocal: 2500, rankingVisitante: 300);

        motor.Simular(partidoDto, new Random(42));

        Assert.IsTrue(partidoDto.GolesLocal >= 1 && partidoDto.GolesLocal <= 3);
        Assert.IsTrue(partidoDto.GolesVisitante >= 0 && partidoDto.GolesVisitante <= 1);
    }
}