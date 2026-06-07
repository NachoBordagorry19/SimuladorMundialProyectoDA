using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioRankingDinamico
{
    void ActualizarRanking(PartidoDTO partidoDto);
}