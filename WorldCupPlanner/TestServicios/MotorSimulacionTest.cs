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
            equipoLocal = new EquipoDTO { nombre = "LocalFC", rankingFifa = rankingLocal, confederacion = Confederacion.UEFA },
            equipoVisitante = new EquipoDTO { nombre = "VisitanteFC", rankingFifa = rankingVisitante, confederacion = Confederacion.CONMEBOL },
            Fecha = DateTime.Now,
            Estadio = new EstadioDTO { Nombre = "Estadio", Ciudad = "Ciudad", Descripcion = "Desc", CapacidadLocativa = 50000 },
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Pendiente
        };
    }

    [TestMethod]
    public void MotorAleatorioPuro_Nombre_EsElCorrecto()
    {
        var motor = new MotorAleatorioPuro();

        Assert.AreEqual("Aleatorio Puro", motor.Nombre);
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

        Assert.IsTrue(partidoDto.golesLocal >= 1 && partidoDto.golesLocal <= 3);
        Assert.IsTrue(partidoDto.golesVisitante >= 0 && partidoDto.golesVisitante <= 1);
    }
}