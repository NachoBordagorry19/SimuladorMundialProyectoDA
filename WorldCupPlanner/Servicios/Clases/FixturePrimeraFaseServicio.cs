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
        int numeroPartido = 0;
        int estadioIndex = 0;
        
        var estadiosDTO = _estadioServicios.ObtenerEstadios();
        
        for (int grupoIndex = 0; grupoIndex < 12; grupoIndex++)
        {
            var grupo = grupos[grupoIndex];
            var equiposDelGrupo = ConvertirEquiposADto(grupo.Equipos);

            int jornada = 0;
            for (int i = 0; i < equiposDelGrupo.Count; i++)
            {
                for (int j = i + 1; j < equiposDelGrupo.Count; j++)
                {
                    var fechaPartido = fechaBase.AddDays(jornada * 3);

                    var partido = new PartidoDTO
                    {
                        idPartido = ++numeroPartido,
                        equipoLocal = equiposDelGrupo[i],
                        equipoVisitante = equiposDelGrupo[j],
                        Fecha = fechaPartido,
                        Estadio = estadiosDTO[estadioIndex % estadiosDTO.Count],
                        fase = Fase.Grupos,
                        estadoPartido = EstadoPartido.Pendiente,
                        golesLocal = 0,
                        golesVisitante = 0
                    };

                    _partidoServicios.AgregarPartido(
                        partido,
                        equiposDelGrupo[i],
                        equiposDelGrupo[j],
                        estadiosDTO[estadioIndex % estadiosDTO.Count]);

                    partidos.Add(partido);
                    estadioIndex++;
                    jornada++;
                }
            }
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
}