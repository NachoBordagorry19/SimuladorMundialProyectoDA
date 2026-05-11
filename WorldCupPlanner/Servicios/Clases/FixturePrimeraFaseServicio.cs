using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.Clases;

public class FixturePrimeraFaseServicio
{
    private readonly IEquipoRepositorio _equipoRepositorio;
    private readonly IEstadioRepositorio _estadioRepositorio;
    private readonly IPartidoRepositorio _partidoRepositorio;
    private readonly EquipoServicios _equipoServicios;
    private readonly EstadioServicios _estadioServicios;
    private readonly PartidoServicios _partidoServicios;

    public FixturePrimeraFaseServicio(
        IEquipoRepositorio equipoRepositorio,
        IEstadioRepositorio estadioRepositorio,
        IPartidoRepositorio partidoRepositorio,
        EquipoServicios equipoServicios,
        EstadioServicios estadioServicios,
        PartidoServicios partidoServicios)
    {
        _equipoRepositorio = equipoRepositorio;
        _estadioRepositorio = estadioRepositorio;
        _partidoRepositorio = partidoRepositorio;
        _equipoServicios = equipoServicios;
        _estadioServicios = estadioServicios;
        _partidoServicios = partidoServicios;
    }

    public ResultadoFixture GenerarFixturePrimeraFase(int semillaFixture, DateTime? fechaInicio = null)
    {
        var equipos = _equipoRepositorio.ObtenerEquipos();
        if (equipos.Count != 48)
        {
            throw new ArgumentException("Debe haber exactamente 48 equipos para generar el fixture");
        }

        var estadios = _estadioRepositorio.ObtenerEstadios();
        if (estadios.Count < 4)
        {
            throw new ArgumentException("Debe haber al menos 4 estadios para generar el fixture");
        }

        var equiposDTO = _equipoServicios.ObtenerEquipos();
        var resultadoOrdenamiento = _equipoServicios.ResolverEmpatesYOrdenar(equiposDTO, semillaFixture);
        var equiposOrdenados = resultadoOrdenamiento.EquiposOrdenados;

        var grupos = CrearGruposVacios();

        for (int bombo = 0; bombo < 4; bombo++)
        {
            for (int pos = 0; pos < 12; pos++)
            {
                var equipoDto = equiposOrdenados[bombo * 12 + pos];
                var equipo = _equipoServicios.EquipoDTOAEntidad(equipoDto);
                int grupoFinal = BuscarGrupoParaEquipo(grupos, equipo, pos);

                grupos[grupoFinal].Equipos.Add(equipo);
            }
        }

        var fechaBase = fechaInicio ?? new DateTime(2026, 06, 01);
        var partidos = new List<PartidoDTO>();
        var estadiosDTO = _estadioServicios.ObtenerEstadios();

        for (int grupoIndex = 0; grupoIndex < grupos.Count; grupoIndex++)
        {
            var grupo = grupos[grupoIndex];
            var fechaInicioGrupo = fechaBase.AddDays(grupoIndex * 9);

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

        return resultado;
    }

    private List<Grupo> CrearGruposVacios()
    {
        var grupos = new List<Grupo>();

        for (int i = 0; i < 12; i++)
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
            if (grupos[i].Equipos.Count < 4)
            {
                return i;
            }
        }

        return grupoPreferido;
    }

    private bool PuedeAgregarseAGrupo(Grupo grupo, Equipo equipo)
    {
        if (grupo.Equipos.Count >= 4)
        {
            return false;
        }

        var confederacion = equipo.Confederacion;
        int cantidadMismaConfederacion = grupo.Equipos.Count(e => e.Confederacion == confederacion);

        if (confederacion == Confederacion.UEFA)
        {
            return cantidadMismaConfederacion < 2;
        }

        return cantidadMismaConfederacion < 1;
    }

    private List<EquipoDTO> ConvertirEquiposADto(List<Equipo> equipos)
    {
        var equiposDto = new List<EquipoDTO>();

        foreach (var equipo in equipos)
        {
            equiposDto.Add(EquipoServicios.EquipoEntidadAEquipoDTO(equipo));
        }

        return equiposDto;
    }

    private void AgregarPartidosDelGrupo(
        Grupo grupo,
        DateTime fechaInicioGrupo,
        List<EstadioDTO> estadios,
        List<PartidoDTO> partidos)
    {
        var equipos = ConvertirEquiposADto(grupo.Equipos);

        var fechaJornada1 = fechaInicioGrupo.Date.AddHours(14);
        var fechaJornada2 = fechaInicioGrupo.Date.AddDays(3).AddHours(14);
        var fechaJornada3 = fechaInicioGrupo.Date.AddDays(6).AddHours(14);

        AgregarPartido(partidos, equipos[0], equipos[3], fechaJornada1, estadios);
        AgregarPartido(partidos, equipos[1], equipos[2], fechaJornada1.AddHours(4), estadios);

        AgregarPartido(partidos, equipos[0], equipos[2], fechaJornada2, estadios);
        AgregarPartido(partidos, equipos[1], equipos[3], fechaJornada2.AddHours(4), estadios);

        AgregarPartido(partidos, equipos[0], equipos[1], fechaJornada3, estadios);
        AgregarPartido(partidos, equipos[2], equipos[3], fechaJornada3, estadios);
    }

    private void AgregarPartido(
        List<PartidoDTO> partidos,
        EquipoDTO equipoLocal,
        EquipoDTO equipoVisitante,
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
}