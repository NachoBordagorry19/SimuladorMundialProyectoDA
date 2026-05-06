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

        if (partidoDto == null)
        {
            throw new ArgumentException("El partido no puede ser nulo porfavor ingrese partido valido");
        }
        
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
    
    public PartidoDTO ObtenerPartido(int id)
    {
        Partido? partido = _partidoRepositorio.ObtenerPartidoPorId(id);
        return PartidoEntidadADto(partido);
    }

    public List<PartidoDTO> ObtenerPartidos()
    {
        if (_partidoRepositorio.ObtenerPartidos().Count == 0)
        {
            throw new ArgumentException("No hay partidos registrados");
        }
        List<Partido> partidos = _partidoRepositorio.ObtenerPartidos();
        List<PartidoDTO> partidosDTO = new List<PartidoDTO>();
        foreach(Partido partido in partidos)
        {
            PartidoDTO partidoDTO = PartidoEntidadADto(partido);
            partidosDTO.Add(partidoDTO);
        }
        return  partidosDTO;
    }
    
    private PartidoDTO PartidoEntidadADto(Partido partido)
    {

        return new PartidoDTO
        {
            Fecha = partido.Fecha,
            Estadio = new EstadioDTO
            {
                Nombre = partido.Estadio.Nombre,
                Ciudad = partido.Estadio.Ciudad,
                Descripcion = partido.Estadio.Descripcion,
                CapacidadLocativa = partido.Estadio.CapacidadLocativa
            },
            equipoLocal = new EquipoDTO
            {
                nombre = partido.Local.Nombre,
                confederacion = partido.Local.Confederacion,
                rankingFifa = partido.Local.RankingFifa
            },
            equipoVisitante = new EquipoDTO
            {
                nombre = partido.Visitante.Nombre,
                confederacion = partido.Visitante.Confederacion,
                rankingFifa = partido.Visitante.RankingFifa
            },
            fase = partido.Fase,
            estadoPartido = partido.Estado,
            golesLocal = partido.GolesLocal,
            golesVisitante = partido.GolesVisitante
        };
    }
}