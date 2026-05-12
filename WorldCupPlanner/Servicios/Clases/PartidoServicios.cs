using Repositorio.Interfaces;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class PartidoServicios : IServicioPartido
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
        
        partido.Grupo = partidoDto.Grupo;

        if (partidoDto.estadoPartido == EstadoPartido.Jugado)
        {
            partido.MarcarComoJugado();
        }

        _partidoRepositorio.AgregarPartido(partido);
        partidoDto.idPartido = partido.Id;
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
        if (_partidoRepositorio.ObtenerPartidoPorId(id) == null)
        {
            throw new ArgumentException("El partido a consultar no existe");
        }
        Partido? partido = _partidoRepositorio.ObtenerPartidoPorId(id);
        return PartidoEntidadADto(partido);
    }

    public List<PartidoDTO> ObtenerPartidos()
    {
        List<Partido> partidos = _partidoRepositorio.ObtenerPartidos();
        if (partidos.Count == 0)
        {
            return new List<PartidoDTO>();
        }
        List<PartidoDTO> partidosDTO = new List<PartidoDTO>();
        foreach (Partido partido in partidos)
        {
            PartidoDTO partidoDTO = PartidoEntidadADto(partido);
            partidosDTO.Add(partidoDTO);
        }
        return partidosDTO;
    }

    public void EliminarPartido(PartidoDTO partidoDTO)
    {
        if (_partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido) == null)
        {
            throw new ArgumentException("El partido a buscar no existe, porfavor busque uno valido");
        }
        var partidoPorId = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido);
        if (partidoPorId != null)
        {
            _partidoRepositorio.EliminarPartido(partidoPorId);
        }
    }

    public void ActualizarPartido(PartidoDTO partidoDTO)
    {

        if (partidoDTO == null)
        {
            throw new ArgumentException("El partido no puede ser nulo");
        }

        Partido partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido);

        if (partidoExistente == null)
        {
            throw new ArgumentException("El partido a actualizar no existe");
        }

        if (partidoDTO.Fecha != default)
        {
            partidoExistente.Fecha = partidoDTO.Fecha;
        }

        if (partidoDTO.Estadio != null)
        {
            partidoExistente.Estadio = EstadioDTOAEntidad(partidoDTO.Estadio);
        }

        if (partidoDTO.golesLocal >= 0 || partidoDTO.golesVisitante >= 0)
        {
            partidoExistente.GolesLocal = partidoDTO.golesLocal;
            partidoExistente.GolesVisitante = partidoDTO.golesVisitante;
            partidoExistente.MarcarComoJugado();
        }
    }

    private PartidoDTO PartidoEntidadADto(Partido partido)
    {

        return new PartidoDTO
        {
            idPartido = partido.Id,
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
            Grupo = partido.Grupo,
            fase = partido.Fase,
            estadoPartido = partido.Estado,
            golesLocal = partido.GolesLocal,
            golesVisitante = partido.GolesVisitante
        };
    }

    private Partido PartidoDTOAEntidad(PartidoDTO dto)
    {
        var estadio = EstadioDTOAEntidad(dto.Estadio);
        var local = EquipoDTOAEntidad(dto.equipoLocal);
        var visitante = EquipoDTOAEntidad(dto.equipoVisitante);
        var fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha;
        var partido = new Partido(fecha, estadio, local, visitante, dto.fase, dto.golesLocal, dto.golesVisitante);
        partido.Grupo = dto.Grupo;
        return partido;
    }

    public void SimularResultado(PartidoDTO partidoDTO, int semillaSimulacion)
    {
        if (partidoDTO == null)
        {
            throw new ArgumentException("El partido recibido no puede ser nulo");
        }

        Partido partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido);

        if (partidoExistente == null)
        {
            throw new ArgumentException("El partido no existe, porfavor ingrese un partido existente");
        }

        Random random = new Random(semillaSimulacion);

        int rankingLocal = partidoExistente.Local.RankingFifa;
        int rankingVisitante = partidoExistente.Visitante.RankingFifa;

        double probabilidadLocal = CalcularProbabilidad(rankingLocal, rankingVisitante);
        double numeroAleatorio = random.NextDouble() * 100;

        int golesLocal;
        int golesVisitante;

        if (numeroAleatorio < probabilidadLocal)
        {
            golesLocal = GenerarGolesAleatorios(random, true);
            golesVisitante = GenerarGolesAleatorios(random, false);
        }
        else
        {
            golesLocal = GenerarGolesAleatorios(random, false);
            golesVisitante = GenerarGolesAleatorios(random, true);
        }

        partidoDTO.golesLocal = golesLocal;
        partidoDTO.golesVisitante = golesVisitante;
        ActualizarPartido(partidoDTO);
    }


    private double CalcularProbabilidad(int rankingLocal, int rankingVisitante)
    {
        double diferencia = rankingVisitante - rankingLocal;
        double probabilidadLocal = (1.0 / (1.0 + Math.Pow(10, diferencia / 400.0))) * 100;
        return probabilidadLocal;
    }


    private int GenerarGolesAleatorios(Random random, bool esEquipoGanador)
    {
        if (esEquipoGanador)
        {
            int numeroAleatorio = random.Next(0, 100);
            if (numeroAleatorio < 50)
                return 1;
            else if (numeroAleatorio < 80)
                return 2;
            else
                return 3;
        }
        else
        {
            int numeroAleatorio = random.Next(0, 100);
            if (numeroAleatorio < 70)
                return 0;
            else
                return 1;
        }
    }


}