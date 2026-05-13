using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioPartido
{
    void AgregarPartido(PartidoDTO partidoDto, EquipoDTO equipoLocal, EquipoDTO equipoVisitante, EstadioDTO estadio);

    PartidoDTO ObtenerPartido(int id);

    List<PartidoDTO> ObtenerPartidos();

    List<PartidoDTO> ObtenerPartidosFiltrados(DateTime? fecha, string estadio, string grupo, string fase);

    List<string> ObtenerEstadiosDePartidos();

    List<string> ObtenerGruposDePartidos();

    List<string> ObtenerFasesDePartidos();

    void ActualizarPartido(PartidoDTO partidoDTO);

    void SimularResultado(PartidoDTO partidoDTO, int semillaSimulacion);

    void SimularTodosLosPartidos(int semillaSimulacion);

    void BloquearEdicionFase(Fase fase);
}