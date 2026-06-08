using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class RankingDinamicoServicio:IServicioRankingDinamico
{
    private readonly IEquipoRepositorio _equipoRepositorio;

    public RankingDinamicoServicio(IEquipoRepositorio equipoRepositorio)
    {
        _equipoRepositorio = equipoRepositorio;
    }

    public void ActualizarRanking(PartidoDTO partidoDto)
    {
        Equipo? local = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == partidoDto.equipoLocal.nombre);
        if (local == null)
        {
            throw new ArgumentException("El equipo local no existe");
        } 
        Equipo? visitante = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == partidoDto.equipoVisitante.nombre);
        if (visitante == null)
        {
            throw new ArgumentException("El equio visitante no existe");
        }

        double probabilidadLocal = 1.0 / (1.0 + Math.Pow(10, (visitante.RankingFifa - local.RankingFifa) / 1000.0));
        double probabilidadVisitante = 1.0 - probabilidadLocal;

        double resultadoLocal = partidoDto.golesLocal > partidoDto.golesVisitante ? 1.0 :
            partidoDto.golesLocal < partidoDto.golesVisitante ? 0.0 : 0.5;
        double resultadoVisitante = 1.0 - resultadoLocal;

        double multiplicador = partidoDto.fase == Fase.Grupos ? 1.0 : 1.5;
        double deltaLocal = 30 * (resultadoLocal - probabilidadLocal) * multiplicador;
        double deltaVisitante = 30 * (resultadoVisitante - probabilidadVisitante) * multiplicador;
        
        local.RankingFifa = Math.Clamp(
            (int)Math.Round(local.RankingFifa + deltaLocal, MidpointRounding.AwayFromZero), 300, 2500);
        visitante.RankingFifa = Math.Clamp(
            (int)Math.Round(visitante.RankingFifa + deltaVisitante, MidpointRounding.AwayFromZero), 300, 2500);
        
        _equipoRepositorio.ActualizarEquipo(local);
        _equipoRepositorio.ActualizarEquipo(visitante);
    }
}