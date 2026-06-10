using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Modelo;
using Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.Clases;

public class FixturePrimeraFaseServicio : IServicioFixturePrimeraFase
{
    private const int TotalEquipos = 48;
    private const int MinimoEstadios = 4;
    private const int CantidadGrupos = 12;
    private const int EquiposPorGrupo = 4;
    private const int CantidadBombos = 4;
    private const int EquiposPorBombo = 12;
    private const int DiasEntreGrupos = 9;
    private const int HoraInicioJornada = 14;
    private const int DiasJornada2 = 3;
    private const int DiasJornada3 = 6;
    private const int HorasEntrePartidos = 4;
    private const int MaxEquiposUEFAPorGrupo = 2;
    private const int MaxEquiposOtrasConfPorGrupo = 1;

    private readonly IEquipoRepositorio _equipoRepositorio;
    private readonly IEstadioRepositorio _estadioRepositorio;
    private readonly IPartidoRepositorio _partidoRepositorio;
    private readonly IServicioEquipo _equipoServicios;
    private readonly IServicioEstadio _estadioServicios;
    private readonly IServicioPartido _partidoServicios;
    private readonly IServicioAuditoria _auditoria;

    public FixturePrimeraFaseServicio(
        IEquipoRepositorio equipoRepositorio,
        IEstadioRepositorio estadioRepositorio,
        IPartidoRepositorio partidoRepositorio,
        IServicioEquipo equipoServicios,
        IServicioEstadio estadioServicios,
        IServicioPartido partidoServicios,
        IServicioAuditoria auditoria)
    {
        _equipoRepositorio = equipoRepositorio;
        _estadioRepositorio = estadioRepositorio;
        _partidoRepositorio = partidoRepositorio;
        _equipoServicios = equipoServicios;
        _estadioServicios = estadioServicios;
        _partidoServicios = partidoServicios;
        _auditoria = auditoria;
    }

    public ResultadoFixture GenerarFixturePrimeraFase(int semillaFixture, DateTime? fechaInicio = null)
    {
        List<Equipo> equipos = _equipoRepositorio.ObtenerEquipos();
        if (equipos.Count != TotalEquipos)
        {
            throw new ArgumentException($"Debe haber exactamente {TotalEquipos} equipos para generar el fixture");
        }

        List<Estadio> estadios = _estadioRepositorio.ObtenerEstadios();
        if (estadios.Count < MinimoEstadios)
        {
            throw new ArgumentException($"Debe haber al menos {MinimoEstadios} estadios para generar el fixture");
        }

        List<EquipoDTO> equiposDTO = _equipoServicios.ObtenerEquipos();
        ResultadoFixture resultadoOrdenamiento = _equipoServicios.ResolverEmpatesYOrdenar(equiposDTO, semillaFixture);
        List<EquipoDTO> equiposOrdenados = resultadoOrdenamiento.EquiposOrdenados;

        List<Grupo> grupos = CrearGruposVacios();

        for (int bombo = 0; bombo < CantidadBombos; bombo++)
        {
            for (int pos = 0; pos < EquiposPorBombo; pos++)
            {
                var equipoDto = equiposOrdenados[bombo * EquiposPorBombo + pos];
                var equipo = _equipoServicios.EquipoDTOAEntidad(equipoDto);
                int grupoFinal = BuscarGrupoParaEquipo(grupos, equipo, pos);

                grupos[grupoFinal].Equipos.Add(equipo);
            }
        }

        DateTime fechaBase = fechaInicio ?? new DateTime(2026, 06, 01);
        var partidos = new List<PartidoDTO>();
        List<EstadioDTO> estadiosDTO = _estadioServicios.ObtenerEstadios()
            .OrderBy(e => NormalizarNombre(e.Nombre))
            .ToList();

        for (int grupoIndex = 0; grupoIndex < grupos.Count; grupoIndex++)
        {
            var grupo = grupos[grupoIndex];
            DateTime fechaInicioGrupo = fechaBase.AddDays(grupoIndex * DiasEntreGrupos);

            AgregarPartidosDelGrupo(
                grupo,
                fechaInicioGrupo,
                estadiosDTO,
                partidos);
        }

        var resultado = new ResultadoFixture
        {
            SemillaFixture = semillaFixture,
            FechaGeneracion = DateTime.UtcNow,
            EquiposOrdenados = resultadoOrdenamiento.EquiposOrdenados,
            AuditoriaEmpates = resultadoOrdenamiento.AuditoriaEmpates,
            Partidos = partidos,
            Grupos = grupos
        };

        _auditoria.RegistrarGeneracionFixture();

        return resultado;
    }

    private List<Grupo> CrearGruposVacios()
    {
        var grupos = new List<Grupo>();

        for (int i = 0; i < CantidadGrupos; i++)
        {
            string nombreGrupo = ((char)('A' + i)).ToString();
            grupos.Add(new Grupo(nombreGrupo));
        }

        return grupos;
    }

    private int BuscarGrupoParaEquipo(List<Grupo> grupos, Equipo equipo, int grupoPreferido)
    {
        if (PuedeAgregarseAGrupo(grupos[grupoPreferido], equipo))
        {
            return grupoPreferido;
        }

        for (int i = 0; i < grupos.Count; i++)
        {
            if (PuedeAgregarseAGrupo(grupos[i], equipo))
            {
                return i;
            }
        }

        for (int i = 0; i < grupos.Count; i++)
        {
            if (grupos[i].Equipos.Count < EquiposPorGrupo)
            {
                return i;
            }
        }

        return grupoPreferido;
    }

    private bool PuedeAgregarseAGrupo(Grupo grupo, Equipo equipo)
    {
        if (grupo.Equipos.Count >= EquiposPorGrupo)
        {
            return false;
        }

        var confederacion = equipo.Confederacion;
        int cantidadMismaConfederacion = grupo.Equipos.Count(e => e.Confederacion == confederacion);

        if (confederacion == Confederacion.UEFA)
        {
            return cantidadMismaConfederacion < MaxEquiposUEFAPorGrupo;
        }

        return cantidadMismaConfederacion < MaxEquiposOtrasConfPorGrupo;
    }

    private List<EquipoDTO> ConvertirEquiposADto(List<Equipo> equipos)
    {
        var equiposDto = new List<EquipoDTO>();

        foreach (var equipo in equipos)
        {
            equiposDto.Add(_equipoServicios.EquipoEntidadAEquipoDTO(equipo));
        }

        return equiposDto;
    }

    private void AgregarPartidosDelGrupo(
        Grupo grupo,
        DateTime fechaInicioGrupo,
        List<EstadioDTO> estadios,
        List<PartidoDTO> partidos)
    {
        List<EquipoDTO> equipos = ConvertirEquiposADto(grupo.Equipos);

        DateTime fechaJornada1 = fechaInicioGrupo.Date.AddHours(HoraInicioJornada);
        DateTime fechaJornada2 = fechaInicioGrupo.Date.AddDays(DiasJornada2).AddHours(HoraInicioJornada);
        DateTime fechaJornada3 = fechaInicioGrupo.Date.AddDays(DiasJornada3).AddHours(HoraInicioJornada);

        AgregarPartido(partidos, equipos[0], equipos[3], grupo.Nombre, fechaJornada1, estadios);
        AgregarPartido(partidos, equipos[1], equipos[2], grupo.Nombre, fechaJornada1.AddHours(HorasEntrePartidos), estadios);

        AgregarPartido(partidos, equipos[0], equipos[2], grupo.Nombre, fechaJornada2, estadios);
        AgregarPartido(partidos, equipos[1], equipos[3], grupo.Nombre, fechaJornada2.AddHours(HorasEntrePartidos), estadios);

        AgregarPartido(partidos, equipos[0], equipos[1], grupo.Nombre, fechaJornada3, estadios);
        AgregarPartido(partidos, equipos[2], equipos[3], grupo.Nombre, fechaJornada3, estadios);
    }

    private void AgregarPartido(
        List<PartidoDTO> partidos,
        EquipoDTO equipoLocal,
        EquipoDTO equipoVisitante,
        string grupo,
        DateTime fechaPartido,
        List<EstadioDTO> estadios)
    {
        int numeroPartido = partidos.Count + 1;
        var estadio = estadios[(numeroPartido - 1) % estadios.Count];

        var partido = new PartidoDTO
        {
            idPartido = numeroPartido,
            equipoLocal = equipoLocal,
            equipoVisitante = equipoVisitante,
            Grupo = grupo,
            Fecha = fechaPartido,
            Estadio = estadio,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Pendiente,
            golesLocal = 0,
            golesVisitante = 0
        };

        _partidoServicios.AgregarPartido(partido, equipoLocal, equipoVisitante, estadio);
        partidos.Add(partido);
    }

    private string NormalizarNombre(string texto)
    {
        return texto.ToLower()
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ü", "u")
            .Trim();
    }
}