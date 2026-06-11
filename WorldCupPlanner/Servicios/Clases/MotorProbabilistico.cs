using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class MotorProbabilistico : IMotorSimulacion
{
    private const double EloDivisor = 400.0;
    private const int ProbabilidadGanadorGol1 = 50;
    private const int ProbabilidadGanadorGol2 = 80;
    private const int ProbabilidadPerdedorGol0 = 70;

    public string Nombre => "Probabilístico";

    public void Simular(PartidoDTO partidoDto, Random random)
    {
        double probabilidadLocal = CalcularProbabilidad(partidoDto.equipoLocal.rankingFifa, partidoDto.equipoVisitante.rankingFifa);
        bool ganaLocal = random.NextDouble() * 100 < probabilidadLocal;

        partidoDto.golesLocal = GenerarGoles(random, ganaLocal);
        partidoDto.golesVisitante = GenerarGoles(random, !ganaLocal);
    }

    private double CalcularProbabilidad(int rankingLocal, int rankingVisitante)
    {
        double diferencia = rankingVisitante - rankingLocal;
        return (1.0 / (1.0 + Math.Pow(10, diferencia / EloDivisor))) * 100;
    }

    private int GenerarGoles(Random random, bool esGanador)
    {
        int numero = random.Next(0, 100);

        if (esGanador)
        {
            if (numero < ProbabilidadGanadorGol1) return 1;
            if (numero < ProbabilidadGanadorGol2) return 2;
            return 3;
        }

        return numero < ProbabilidadPerdedorGol0 ? 0 : 1;
    }
}