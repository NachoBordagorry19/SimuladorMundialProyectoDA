using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class MotorAleatorioPuro : IMotorSimulacion
{
    private const int ProbabilidadGanadorGol1 = 50;
    private const int ProbabilidadGanadorGol2 = 80;
    private const int ProbabilidadPerdedorGol0 = 70;

    public string Nombre => "Aleatorio Puro";

    public void Simular(PartidoDTO partidoDto, Random random)
    {
        bool ganaLocal = random.Next(0, 2) == 0;

        partidoDto.GolesLocal = GenerarGoles(random, ganaLocal);
        partidoDto.GolesVisitante = GenerarGoles(random, !ganaLocal);
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