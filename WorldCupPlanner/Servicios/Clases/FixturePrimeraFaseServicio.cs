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
        
        var grupos = new List<List<EquipoDTO>>();
        for (int grupo = 0; grupo < 12; grupo++)
        {
            grupos.Add(new List<EquipoDTO>());
        }
        
        for (int bombo = 0; bombo < 4; bombo++)
        {
            for (int pos = 0; pos < 12; pos++)
            {
                var equipo = equiposOrdenados[bombo * 12 + pos];
                var grupoPreferido = pos;
                var grupoFinal = grupoPreferido;
                
                if (!PuedeAgregarseAGrupo(grupos[grupoPreferido], equipo))
                {
                    grupoFinal = -1;
                    for (int g = 0; g < 12; g++)
                    {
                        if (PuedeAgregarseAGrupo(grupos[g], equipo))
                        {
                            grupoFinal = g;
                            break;
                        }
                    }
                    
                    if (grupoFinal == -1)
                    {
                        grupoFinal = grupoPreferido;
                    }
                }

                grupos[grupoFinal].Add(equipo);
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
            char letraGrupo = (char)('A' + grupoIndex);
            
            for (int i = 0; i < grupo.Count; i++)
            {
                for (int j = i + 1; j < grupo.Count; j++)
                {
                    var partido = new PartidoDTO
                    {
                        idPartido = ++numeroPartido,
                        equipoLocal = grupo[i],
                        equipoVisitante = grupo[j],
                        Fecha = fechaBase.AddDays(numeroPartido / 2),
                        Estadio = estadiosDTO[estadioIndex % estadiosDTO.Count],
                        fase = Fase.Grupos,
                        estadoPartido = EstadoPartido.Pendiente,
                        golesLocal = 0,
                        golesVisitante = 0
                    };
                    
                    _partidoServicios.AgregarPartido(partido, grupo[i], grupo[j], estadiosDTO[estadioIndex % estadiosDTO.Count]);
                    partidos.Add(partido);
                    estadioIndex++;
                }
            }
        }
        
        var resultado = new ResultadoFixture
        {
            SemillaFixture = semillaFixture,
            FechaGeneracion = DateTime.UtcNow,
            EquiposOrdenados = resultadoOrdenamiento.EquiposOrdenados,
            AuditoriaEmpates = resultadoOrdenamiento.AuditoriaEmpates,
            Partidos = partidos
        };

        return resultado;
    }

    private bool PuedeAgregarseAGrupo(List<EquipoDTO> grupo, EquipoDTO equipo)
    {
        var confederacion = equipo.confederacion;
        int conteo = grupo.Count(e => e.confederacion == confederacion);

        if (confederacion == Confederacion.UEFA)
        {
            return conteo < 2;
        }
        else
        {
            return conteo < 1;
        }
    }
}