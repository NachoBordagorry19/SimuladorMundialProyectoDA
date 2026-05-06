using Repositorio.Interfaces;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;

namespace Servicios.Clases;

public class PartidoServicios
{
    private readonly IPartidoRepositorio _partidoRepositorio;

    public PartidoServicios(IPartidoRepositorio partidoRepositorio)
    {
        _partidoRepositorio = partidoRepositorio;
    }
    
    public void AgregarPartido(PartidoDTO partidoDto, EquipoDTO equipoLocal, EquipoDTO equipoVisitante, EstadioDTO estadio)
    {
        
        var partido = new Partido(
            partidoDto.Fecha == default ? DateTime.UtcNow : partidoDto.Fecha,
            EstadioDTOAEntidad(estadio), 
            EquipoDTOAEntidad(equipoLocal),
            EquipoDTOAEntidad(equipoVisitante),
            partidoDto.fase,
            partidoDto.golesLocal,
            partidoDto.golesVisitante
        );

        _partidoRepositorio.AgregarPartido(partido);
    }
    
    public Equipo EquipoDTOAEntidad(EquipoDTO equipoDto)
    {
        return new Equipo(
            equipoDto.nombre,
            equipoDto.confederacion,
            equipoDto.rankingFifa
        );
    }
    
    private Estadio EstadioDTOAEntidad(EstadioDTO estadioDTO)
    {
        var estadio = new Estadio(
            estadioDTO.Nombre,
            estadioDTO.Ciudad,
            estadioDTO.Descripcion,
            estadioDTO.CapacidadLocativa
        );

        return estadio;
    }
    
    
}