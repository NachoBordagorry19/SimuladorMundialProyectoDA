using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioPartido
{
    void AgregarPartido(PartidoDTO partidoDto, EquipoDTO equipoLocal, EquipoDTO equipoVisitante, EstadioDTO estadio);

    PartidoDTO ObtenerPartido(int id);

    List<PartidoDTO> ObtenerPartidos();

    void ActualizarPartido(PartidoDTO partidoDTO);

    void BloquearEdicionFase(Fase fase);
}