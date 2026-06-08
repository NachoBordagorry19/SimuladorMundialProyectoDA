using Repositorio.Interfaces;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class PartidoServicios : IServicioPartido
{
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
        
        var partido = new Partido(fecha, estadioEntidad, localEntidad, visitanteEntidad, partidoDto.fase, partidoDto.golesLocal, partidoDto.golesVisitante);
        
        partido.Grupo = partidoDto.Grupo;
        
        if (partidoDto.incidenciaEquipoLocal != null && partidoDto.incidenciaEquipoLocal.Count > 0)
        {
            partido.incidenciaEquipoLocal = new List<TipoIncidencia>(partidoDto.incidenciaEquipoLocal);
        }
        if (partidoDto.incidenciaEquipoVisitante != null && partidoDto.incidenciaEquipoVisitante.Count > 0)
        {
            partido.incidenciaEquipoVisitante = new List<TipoIncidencia>(partidoDto.incidenciaEquipoVisitante);
        }

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

            if (!string.IsNullOrWhiteSpace(fase) && partido.fase.ToString() != fase)
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
            string fase = partido.fase.ToString();

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

        Partido? partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido);
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

        if (partidoDTO.golesLocal >= 0 || partidoDTO.golesVisitante >= 0)
        {
            bool eraPendiente = partidoExistente.Estado == EstadoPartido.Pendiente;
            partidoExistente.GolesLocal = partidoDTO.golesLocal;
            partidoExistente.GolesVisitante = partidoDTO.golesVisitante;
            partidoExistente.MarcarComoJugado();
            if (eraPendiente)
                _rankingDinamico.ActualizarRanking(PartidoEntidadADto(partidoExistente));
        }
        if (partidoDTO.incidenciaEquipoLocal != null)
            partidoExistente.incidenciaEquipoLocal = new List<TipoIncidencia>(partidoDTO.incidenciaEquipoLocal);
        if (partidoDTO.incidenciaEquipoVisitante != null)
            partidoExistente.incidenciaEquipoVisitante = new List<TipoIncidencia>(partidoDTO.incidenciaEquipoVisitante);

        string detalle = $"{partidoDTO.equipoLocal.nombre} vs {partidoDTO.equipoVisitante.nombre}";
        _auditoria.RegistrarModificacionPartido(detalle);
        _partidoRepositorio.ActualizarPartido(partidoExistente);
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
            golesVisitante = partido.GolesVisitante,
            incidenciaEquipoLocal = new List<TipoIncidencia>(partido.incidenciaEquipoLocal ?? new List<TipoIncidencia>()),
            incidenciaEquipoVisitante = new List<TipoIncidencia>(partido.incidenciaEquipoVisitante ?? new List<TipoIncidencia>())
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
        if (dto.incidenciaEquipoLocal != null)
        {
            partido.incidenciaEquipoLocal = new List<TipoIncidencia>(dto.incidenciaEquipoLocal);
        }
        if (dto.incidenciaEquipoVisitante != null)
        {
            partido.incidenciaEquipoVisitante = new List<TipoIncidencia>(dto.incidenciaEquipoVisitante);
        }
        return partido;
    }

    public void SimularResultado(PartidoDTO partidoDTO, int semillaSimulacion)
    {
        if (partidoDTO == null)
        {
            throw new ArgumentException("El partido recibido no puede ser nulo");
        }

        Partido? partidoExistente = _partidoRepositorio.ObtenerPartidoPorId(partidoDTO.idPartido);
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

        bool ganaLocal = numeroAleatorio < probabilidadLocal;

        if (ganaLocal)
        {
            golesLocal = GenerarGolesAleatorios(random, true);
            golesVisitante = GenerarGolesAleatorios(random, false);
        }
        else
        {
            golesLocal = GenerarGolesAleatorios(random, false);
            golesVisitante = GenerarGolesAleatorios(random, true);
        }

        if (partidoExistente.Fase != Fase.Grupos && golesLocal == golesVisitante)
        {
            if (ganaLocal)
            {
                golesLocal++;
            }
            else
            {
                golesVisitante++;
            }
        }

        int tarjetasAmarillasLocal = random.Next(0, 6);
        for (int i = 0; i < tarjetasAmarillasLocal; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaAmarilla, true);

        int tarjetasAmarillasVisitante = random.Next(0, 6);
        for (int i = 0; i < tarjetasAmarillasVisitante; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaAmarilla, false);

        int tarjetasRojasLocal = random.Next(0, 100) < 30 ? random.Next(1, 4) : 0;
        for (int i = 0; i < tarjetasRojasLocal; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaRoja, true);

        int tarjetasRojasVisitante = random.Next(0, 100) < 30 ? random.Next(1, 4) : 0;
        for (int i = 0; i < tarjetasRojasVisitante; i++)
            partidoExistente.AgregarIncidencia(TipoIncidencia.TarjetaRoja, false);

        partidoDTO.golesLocal = golesLocal;
        partidoDTO.golesVisitante = golesVisitante;
        partidoDTO.incidenciaEquipoLocal = new List<TipoIncidencia>(partidoExistente.incidenciaEquipoLocal);
        partidoDTO.incidenciaEquipoVisitante = new List<TipoIncidencia>(partidoExistente.incidenciaEquipoVisitante);
        ActualizarPartido(partidoDTO);
        _auditoria.RegistrarSimulacion(semillaSimulacion);
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

    public void SimularTodosLosPartidos(int semillaSimulacion)
    {
        List<PartidoDTO> partidos = ObtenerPartidos();

        foreach (PartidoDTO partido in partidos)
        {
            if (partido.estadoPartido != EstadoPartido.Jugado || EsPartidoEliminatorioEmpatado(partido))
            {
                SimularResultado(partido, semillaSimulacion + partido.idPartido);
            }
        }
        _auditoria.RegistrarSimulacion(semillaSimulacion);
    }
    private bool EsPartidoEliminatorioEmpatado(PartidoDTO partido)
    {
        return partido.fase != Fase.Grupos &&
               partido.estadoPartido == EstadoPartido.Jugado &&
               partido.golesLocal == partido.golesVisitante;
    }


}