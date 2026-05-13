using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioFixturePrimeraFase
{
    ResultadoFixture GenerarFixturePrimeraFase(int semillaFixture, DateTime? fechaInicio = null);
}