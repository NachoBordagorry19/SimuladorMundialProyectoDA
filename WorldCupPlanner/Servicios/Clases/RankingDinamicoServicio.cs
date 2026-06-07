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

        if (partidoDto.golesLocal == partidoDto.golesVisitante)
        {
            return;
        }
        _equipoRepositorio.ActualizarEquipo(local);
        _equipoRepositorio.ActualizarEquipo(visitante);
    }
}