using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IMotorSimulacion
{
    string Nombre { get; }
    void Simular(PartidoDTO partidoDto, Random random);
}