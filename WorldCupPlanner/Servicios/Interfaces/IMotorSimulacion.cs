using Dominio.Clases;

namespace Servicios.Interfaces;

public interface IMotorSimulacion
{
    string Nombre { get; }
    (int golesLocal, int golesVisitante) Simular(Partido partido, Random random);
}