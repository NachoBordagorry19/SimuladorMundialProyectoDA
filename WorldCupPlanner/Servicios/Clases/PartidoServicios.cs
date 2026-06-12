using Repositorio.Interfaces;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class PartidoServicios : IServicioPartido
{
    private const int MaxTarjetasAmarillas = 6;
    private const int ProbabilidadTarjetaRoja = 30;
    private const int MaxTarjetasRojas = 3;

    private readonly IPartidoRepositorio _partidoRepositorio;
    private readonly IServicioAuditoria _auditoria;
    private readonly List<Fase> _fasesBloqueadas = new List<Fase>();
    private readonly IServicioRankingDinamico _rankingDinamico;

    public PartidoServicios(IPartidoRepositorio partidoRepositorio, IServicioAuditoria auditoria, IServicioRankingDinamico rankingDinamico)
    {
        _partidoRepositorio = partidoRepositorio;
        _auditoria = auditoria;
        _rankingDinamico = rankingDinamico;
    }

    public void AgregarPartido(PartidoDTO partidoDto, EquipoDTO equipoLocal, EquipoDTO equipoVisitante, EstadioDTO estadio)
    {

        if (partidoDto == null)
        {
            throw new ArgumentException("El partido no puede ser nulo porfavor ingrese partido valido");
        }

        var estadioEntidad = EstadioDTOAEntidad(estadio);
        var localEntidad = EquipoDTOAEntidad(equipoLocal);
        var visitanteEntidad = EquipoDTOAEntidad(equipoVisitante);
        var fecha = partidoDto.Fecha == default ? DateTime.UtcNow : partidoDto.Fecha;
        
        var partido = new Partido(fecha, estadioEntidad, localEntidad, visitanteEntidad, partidoDto.Fase, partidoDto.GolesLocal, partidoDto.GolesVisitante);
        
        partido.Grupo = partidoDto.Grupo;
        
        if (partidoDto.IncidenciaEquipoLocal != null && partidoDto.IncidenciaEquipoLocal.Count > 0)
        {
            partido.incidenciaEquipoLocal = new List<TipoIncidencia>(partidoDto.IncidenciaEquipoLocal);
        }
        if (partidoDto.IncidenciaEquipoVisitante != null && partidoDto.IncidenciaEquipoVisitante.Count > 0)
        {
            partido.incidenciaEquipoVisitante = new List<TipoIncidencia>(partidoDto.IncidenciaEquipoVisitante);
        }

        if (partidoDto.EstadoPartido == EstadoPartido.Jugado)
        {
            partido.MarcarComoJugado();
        }

        _partidoRepositorio.AgregarPartido(partido);
        partidoDto.IdPartido = partido.Id;
    }

    public Equipo EquipoDTOAEntidad(EquipoDTO equipoDto)
    {
        return new Equipo(
            equipoDto.Nombre,
            equipoDto.Confederacion,
            equipoDto.RankingFifa
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
        var partido = _partidoRepositorio.ObtenerPartidoPorId(id);
        if (partido == null)
        {
            throw new ArgumentException("El partido a consultar no existe");
        }
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

    public List<PartidoDTO> ObtenerPartidosFiltrados(DateTime? fecha, string estadio, string grupo, string fase)
    {
        List<PartidoDTO> partidos = ObtenerPartidos();
        List<PartidoDTO> partidosFiltrados = new List<PartidoDTO>();

        foreach (PartidoDTO partido in partidos)
        {
            bool cumpleFiltros = true;

            if (fecha != null && partido.Fecha.Date != fecha.Value.Date)
            {
                cumpleFiltros = false;
            }

            if (!string.IsNullOrWhiteSpace(estadio) && partido.Estadio.Nombre != estadio)
            {
                cumpleFiltros = false;
            }

            if (!string.IsNullOrWhiteSpace(grupo) && partido.Grupo != grupo)
            {
                cumpleFiltros = false;
            }

            if (!string.IsNullOrWhiteSpace(fase) && partido.Fase.ToString() != fase)
            {
                cumpleFiltros = false;
            }

            if (cumpleFiltros)
            {
                partidosFiltrados.Add(partido);
            }
        }

        return partidosFiltrados
            .OrderBy(p => p.Fecha)
            .ToList();
    }

    public List<string> ObtenerEstadiosDePartidos()
    {
        List<PartidoDTO> partidos = ObtenerPartidos();
        List<string> estadios = new List<string>();

        foreach (PartidoDTO partido in partidos)
        {
            if (!estadios.Contains(partido.Estadio.Nombre))
            {
                estadios.Add(partido.Estadio.Nombre);
            }
        }

        estadios.Sort();

        return estadios;
    }

    public List<string> ObtenerGruposDePartidos()
    {
        List<PartidoDTO> partidos = ObtenerPartidos();
        List<string> grupos = new List<string>();

        foreach (PartidoDTO partido in partidos)
        {
            if (!string.IsNullOrWhiteSpace(partido.Grupo) && !grupos.Contains(partido.Grupo))
            {
                grupos.Add(partido.Grupo);
            }
        }

        grupos.Sort();

        return grupos;
    }

    public List<string> ObtenerFasesDePartidos()
    {
        List<PartidoDTO> partidos = ObtenerPartidos();
        List<string> fases = new List<string>();

        foreach (PartidoDTO partido in partidos)
        {
            string fase = partido.Fase.ToString();

            if (!fases.Contains(fase))
            {
                fases.Add(fase);
            }
        }

        fases.Sort();

        return fases;
    }

    public void EliminarPartido(PartidoDTO partidoDTO)
    {
        if (_partidoRepositorio.ObtenerPartidoPorId(partidoDTO.IdPartido) == null)
        {
            throw new ArgumentException("El partido a buscar no existe, porfavor busque uno valido");
        }
        var partidoPorId = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.IdPartido);
        if (partidoPorId != null)
        {
            _partidoRepositorio.EliminarPartido(partidoPorId);
        }
    }

    public void BloquearEdicionFase(Fase fase)
    {
        if (!_fasesBloqueadas.Contains(fase))
        {
            _fasesBloqueadas.Add(fase);
        }
    }

    public void ActualizarPartido(PartidoDTO partidoDTO)
    {

        if (partidoDTO == null)
        {
            throw new ArgumentException("El partido no puede ser nulo");
        }

        Partido? partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.IdPartido);
        if (partidoExistente == null)
        {
            throw new ArgumentException("El partido a actualizar no existe");
        }

        if (_fasesBloqueadas.Contains(partidoExistente.Fase))
        {
            throw new ArgumentException("No se puede editar un partido de una fase anterior");
        }

        if (partidoDTO.Fecha != default)
        {
            partidoExistente.Fecha = partidoDTO.Fecha;
        }

        if (partidoDTO.Estadio != null)
        {
            partidoExistente.Estadio = EstadioDTOAEntidad(partidoDTO.Estadio);
        }

        if (partidoDTO.GolesLocal >= 0 || partidoDTO.GolesVisitante >= 0)
        {
            bool eraPendiente = partidoExistente.Estado == EstadoPartido.Pendiente;
            partidoExistente.GolesLocal = partidoDTO.GolesLocal;
            partidoExistente.GolesVisitante = partidoDTO.GolesVisitante;
            partidoExistente.MarcarComoJugado();
            if (eraPendiente)
                _rankingDinamico.ActualizarRanking(PartidoEntidadADto(partidoExistente));
        }
        if (partidoDTO.IncidenciaEquipoLocal != null)
            partidoExistente.incidenciaEquipoLocal = new List<TipoIncidencia>(partidoDTO.IncidenciaEquipoLocal);
        if (partidoDTO.IncidenciaEquipoVisitante != null)
            partidoExistente.incidenciaEquipoVisitante = new List<TipoIncidencia>(partidoDTO.IncidenciaEquipoVisitante);

        string detalle = $"{partidoDTO.EquipoLocal.Nombre} vs {partidoDTO.EquipoVisitante.Nombre}";
        _auditoria.RegistrarModificacionPartido(detalle);
        _partidoRepositorio.ActualizarPartido(partidoExistente);
    }

    private PartidoDTO PartidoEntidadADto(Partido partido)
    {

        return new PartidoDTO
        {
            IdPartido = partido.Id,
            Fecha = partido.Fecha,
            Estadio = new EstadioDTO
            {
                Nombre = partido.Estadio.Nombre,
                Ciudad = partido.Estadio.Ciudad,
                Descripcion = partido.Estadio.Descripcion,
                CapacidadLocativa = partido.Estadio.CapacidadLocativa
            },
            EquipoLocal = new EquipoDTO
            {
                Nombre = partido.Local.Nombre,
                Confederacion = partido.Local.Confederacion,
                RankingFifa = partido.Local.RankingFifa
            },
            EquipoVisitante = new EquipoDTO
            {
                Nombre = partido.Visitante.Nombre,
                Confederacion = partido.Visitante.Confederacion,
                RankingFifa = partido.Visitante.RankingFifa
            },
            Grupo = partido.Grupo,
            Fase = partido.Fase,
            EstadoPartido = partido.Estado,
            GolesLocal = partido.GolesLocal,
            GolesVisitante = partido.GolesVisitante,
            IncidenciaEquipoLocal = new List<TipoIncidencia>(partido.incidenciaEquipoLocal ?? new List<TipoIncidencia>()),
            IncidenciaEquipoVisitante = new List<TipoIncidencia>(partido.incidenciaEquipoVisitante ?? new List<TipoIncidencia>())
        };
    }

    private Partido PartidoDTOAEntidad(PartidoDTO dto)
    {
        var estadio = EstadioDTOAEntidad(dto.Estadio);
        var local = EquipoDTOAEntidad(dto.EquipoLocal);
        var visitante = EquipoDTOAEntidad(dto.EquipoVisitante);
        var fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha;
        var partido = new Partido(fecha, estadio, local, visitante, dto.Fase, dto.GolesLocal, dto.GolesVisitante);
        partido.Grupo = dto.Grupo;
        if (dto.IncidenciaEquipoLocal != null)
        {
            partido.incidenciaEquipoLocal = new List<TipoIncidencia>(dto.IncidenciaEquipoLocal);
        }
        if (dto.IncidenciaEquipoVisitante != null)
        {
            partido.incidenciaEquipoVisitante = new List<TipoIncidencia>(dto.IncidenciaEquipoVisitante);
        }
        return partido;
    }

    public void SimularResultado(PartidoDTO partidoDTO, int semillaSimulacion, IMotorSimulacion motor)
    {
        if (partidoDTO == null)
        {
            throw new ArgumentException("El partido recibido no puede ser nulo");
        }

        Partido? partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.IdPartido);
        if (partidoExistente == null)
        {
            throw new ArgumentException("El partido no existe, porfavor ingrese un partido existente");
        }

        Random random = new Random(semillaSimulacion);

        motor.Simular(partidoDTO, random);

        if (partidoExistente.Fase != Fase.Grupos && partidoDTO.GolesLocal == partidoDTO.GolesVisitante)
        {
            partidoDTO.GolesLocal++;
        }

        int tarjetasAmarillasLocal = random.Next(0, MaxTarjetasAmarillas);
        for (int i = 0; i < tarjetasAmarillasLocal; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaAmarilla, true);

        int tarjetasAmarillasVisitante = random.Next(0, MaxTarjetasAmarillas);
        for (int i = 0; i < tarjetasAmarillasVisitante; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaAmarilla, false);

        int tarjetasRojasLocal = random.Next(0, 100) < ProbabilidadTarjetaRoja ? random.Next(1, MaxTarjetasRojas + 1) : 0;
        for (int i = 0; i < tarjetasRojasLocal; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaRoja, true);

        int tarjetasRojasVisitante = random.Next(0, 100) < ProbabilidadTarjetaRoja ? random.Next(1, MaxTarjetasRojas + 1) : 0;
        for (int i = 0; i < tarjetasRojasVisitante; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaRoja, false);

        partidoDTO.IncidenciaEquipoLocal = new List<TipoIncidencia>(partidoExistente.incidenciaEquipoLocal);
        partidoDTO.IncidenciaEquipoVisitante = new List<TipoIncidencia>(partidoExistente.incidenciaEquipoVisitante);
        ActualizarPartido(partidoDTO);
        partidoDTO.EstadoPartido = EstadoPartido.Jugado;
        _auditoria.RegistrarSimulacion(semillaSimulacion);
    }


    public void SimularTodosLosPartidos(int semillaSimulacion, IMotorSimulacion motor)
    {
        List<PartidoDTO> partidos = ObtenerPartidos();

        foreach (PartidoDTO partido in partidos)
        {
            if (partido.EstadoPartido != EstadoPartido.Jugado || EsPartidoEliminatorioEmpatado(partido))
            {
                SimularResultado(partido, semillaSimulacion + partido.IdPartido, motor);
                ActualizarPartido(partido);
            }
        }
        _auditoria.RegistrarSimulacion(semillaSimulacion);
    }
    private bool EsPartidoEliminatorioEmpatado(PartidoDTO partido)
    {
        return partido.Fase != Fase.Grupos &&
               partido.EstadoPartido == EstadoPartido.Jugado &&
               partido.GolesLocal == partido.GolesVisitante;
    }


}