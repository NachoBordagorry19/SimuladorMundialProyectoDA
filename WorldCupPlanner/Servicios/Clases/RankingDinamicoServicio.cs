using Dominio.Clases;
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
    }
}