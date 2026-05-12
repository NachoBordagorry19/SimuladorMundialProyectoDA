using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioPartido
{
    void AgregarPartido(PartidoDTO partidoDto, EquipoDTO equipoLocal, EquipoDTO equipoVisitante, EstadioDTO estadio);
}