using Dominio.Enums;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class CrucesSegundaFaseServicio : IServicioCrucesSegundaFase
{
    private readonly IServicioPartido _partidoServicios;
    private readonly IServicioAuditoria _auditoria;

    public CrucesSegundaFaseServicio(IServicioPartido partidoServicios,  IServicioAuditoria auditoria)
    {
        _partidoServicios = partidoServicios;
        _auditoria = auditoria;
    }

    public List<PosicionEquipoDTO> ObtenerRankingGrupo(string grupo, int semillaCrucesFase)
    {
        var partidos = _partidoServicios.ObtenerPartidos()
            .Where(p => p.Grupo == grupo && p.fase == Fase.Grupos)
            .ToList();

        var posiciones = new List<PosicionEquipoDTO>();

        foreach (var partido in partidos)
        {
            AgregarEquipoSiNoExiste(posiciones, partido.equipoLocal, grupo);
            AgregarEquipoSiNoExiste(posiciones, partido.equipoVisitante, grupo);

            if (partido.estadoPartido != EstadoPartido.Jugado)
            {
                continue;
            }

            var posicionLocal = BuscarPosicion(posiciones, partido.equipoLocal.nombre);
            var posicionVisitante = BuscarPosicion(posiciones, partido.equipoVisitante.nombre);

            posicionLocal.PartidosJugados++;
            posicionVisitante.PartidosJugados++;

            posicionLocal.GolesAFavor += partido.golesLocal;
            posicionLocal.GolesEnContra += partido.golesVisitante;

            posicionVisitante.GolesAFavor += partido.golesVisitante;
            posicionVisitante.GolesEnContra += partido.golesLocal;

            if (partido.golesLocal > partido.golesVisitante)
            {
                posicionLocal.Ganados++;
                posicionLocal.Puntos += 3;

                posicionVisitante.Perdidos++;
            }
            else if (partido.golesVisitante > partido.golesLocal)
            {
                posicionVisitante.Ganados++;
                posicionVisitante.Puntos += 3;

                posicionLocal.Perdidos++;
            }
            else
            {
                posicionLocal.Empatados++;
                posicionVisitante.Empatados++;

                posicionLocal.Puntos++;
                posicionVisitante.Puntos++;
            }

            posicionLocal.Diferencia = posicionLocal.GolesAFavor - posicionLocal.GolesEnContra;
            posicionVisitante.Diferencia = posicionVisitante.GolesAFavor - posicionVisitante.GolesEnContra;
        }

        return OrdenarPosiciones(posiciones, semillaCrucesFase);
    }

    private void AgregarEquipoSiNoExiste(List<PosicionEquipoDTO> posiciones, EquipoDTO equipo, string grupo)
    {
        if (posiciones.Any(p => p.EquipoNombre == equipo.nombre))
        {
            return;
        }

        posiciones.Add(new PosicionEquipoDTO
        {
            EquipoNombre = equipo.nombre,
            Grupo = grupo,
            Equipo = equipo
        });
    }

    private PosicionEquipoDTO BuscarPosicion(List<PosicionEquipoDTO> posiciones, string nombreEquipo)
    {
        return posiciones.First(p => p.EquipoNombre == nombreEquipo);
    }

    private List<PosicionEquipoDTO> AplicarSorteoEnEmpates(List<PosicionEquipoDTO> posiciones, int semillaCrucesFase)
    {
        var resultado = new List<PosicionEquipoDTO>();
        var random = new Random(semillaCrucesFase);

        int i = 0;

        while (i < posiciones.Count)
        {
            var grupoEmpatado = new List<PosicionEquipoDTO>();
            var posicionActual = posiciones[i];

            grupoEmpatado.Add(posicionActual);
            i++;

            while (i < posiciones.Count && EstanEmpatados(posicionActual, posiciones[i]))
            {
                grupoEmpatado.Add(posiciones[i]);
                i++;
            }

            if (grupoEmpatado.Count > 1)
            {
                MezclarFisherYates(grupoEmpatado, random);
            }

            resultado.AddRange(grupoEmpatado);
        }

        return resultado;
    }

    private bool EstanEmpatados(PosicionEquipoDTO equipoA, PosicionEquipoDTO equipoB)
    {
        return equipoA.Puntos == equipoB.Puntos &&
               equipoA.Diferencia == equipoB.Diferencia &&
               equipoA.GolesAFavor == equipoB.GolesAFavor;
    }

    private void MezclarFisherYates(List<PosicionEquipoDTO> equipos, Random random)
    {
        for (int i = equipos.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            var auxiliar = equipos[i];
            equipos[i] = equipos[j];
            equipos[j] = auxiliar;
        }
    }

    public ClasificadosDTO ObtenerClasificados(int semillaCrucesFase)
    {
        var primeros = new List<PosicionEquipoDTO>();
        var segundos = new List<PosicionEquipoDTO>();
        var terceros = new List<PosicionEquipoDTO>();

        for (char letraGrupo = 'A'; letraGrupo <= 'L'; letraGrupo++)
        {
            string grupo = letraGrupo.ToString();

            int partidosJugados = _partidoServicios.ObtenerPartidos()
                .Count(p => p.Grupo == grupo &&
                            p.fase == Fase.Grupos &&
                            p.estadoPartido == EstadoPartido.Jugado);

            if (partidosJugados < 6)
            {
                throw new ArgumentException("No se pueden generar cruces hasta que todos los grupos tengan sus partidos jugados.");
            }

            var rankingGrupo = ObtenerRankingGrupo(grupo, semillaCrucesFase);

            primeros.Add(rankingGrupo[0]);
            segundos.Add(rankingGrupo[1]);
            terceros.Add(rankingGrupo[2]);
        }

        var clasificados = new ClasificadosDTO
        {
            Primeros = OrdenarPosiciones(primeros, semillaCrucesFase),
            Segundos = OrdenarPosiciones(segundos, semillaCrucesFase),
            Terceros = OrdenarPosiciones(terceros, semillaCrucesFase).Take(8).ToList()
        };

        return clasificados;
    }

    private List<PosicionEquipoDTO> OrdenarPosiciones(List<PosicionEquipoDTO> posiciones, int semillaCrucesFase)
    {
        var posicionesOrdenadas = posiciones
            .OrderByDescending(p => p.Puntos)
            .ThenByDescending(p => p.Diferencia)
            .ThenByDescending(p => p.GolesAFavor)
            .ToList();

        return AplicarSorteoEnEmpates(posicionesOrdenadas, semillaCrucesFase);
    }

    public List<CruceDTO> GenerarCrucesEntreListas(
        List<PosicionEquipoDTO> equiposLocales,
        List<PosicionEquipoDTO> equiposVisitantes,
        string prefijoCodigo,
        int semillaCrucesFase,
        int numeroInicial = 1)
    {
        var locales = equiposLocales.ToList();
        var visitantes = equiposVisitantes.ToList();

        var random = new Random(semillaCrucesFase);

        MezclarFisherYates(locales, random);
        MezclarFisherYates(visitantes, random);

        var cruces = new List<CruceDTO>();

        for (int i = 0; i < locales.Count; i++)
        {
            var local = locales[i];
            int indiceVisitante = BuscarIndiceVisitanteDisponible(visitantes, local);

            var visitante = visitantes[indiceVisitante];
            visitantes.RemoveAt(indiceVisitante);

            var cruce = new CruceDTO
            {
                Codigo = prefijoCodigo + (numeroInicial + i),
                Fase = Fase.Dieciseisavos,
                EquipoLocal = local,
                EquipoVisitante = visitante
            };

            cruces.Add(cruce);
        }

        return cruces;
    }

    private int BuscarIndiceVisitanteDisponible(List<PosicionEquipoDTO> visitantes, PosicionEquipoDTO local)
    {
        for (int i = 0; i < visitantes.Count; i++)
        {
            if (visitantes[i].Grupo != local.Grupo)
            {
                return i;
            }
        }

        return 0;
    }

    public List<CruceDTO> GenerarCrucesFase(ClasificadosDTO clasificados, int semillaCrucesFase)
    {
        var mejoresPrimeros = clasificados.Primeros
            .Take(8)
            .ToList();

        var terceros = clasificados.Terceros
            .ToList();

        var cruces = GenerarCrucesEntreListas(
            mejoresPrimeros,
            terceros,
            "A",
            semillaCrucesFase);

        var primerosRestantes = clasificados.Primeros
            .Skip(8)
            .Take(4)
            .ToList();

        var segundosMenorPuntaje = clasificados.Segundos
            .OrderBy(s => s.Puntos)
            .Take(4)
            .ToList();

        var crucesB1aB4 = GenerarCrucesEntreListas(
            primerosRestantes,
            segundosMenorPuntaje,
            "B",
            semillaCrucesFase);

        cruces.AddRange(crucesB1aB4);

        var segundosRestantes = clasificados.Segundos
            .Where(s => !segundosMenorPuntaje.Any(m => m.EquipoNombre == s.EquipoNombre))
            .ToList();

        var segundosLocales = segundosRestantes
            .Take(4)
            .ToList();

        var segundosVisitantes = segundosRestantes
            .Skip(4)
            .Take(4)
            .ToList();

        var crucesB5aB8 = GenerarCrucesEntreListas(
            segundosLocales,
            segundosVisitantes,
            "B",
            semillaCrucesFase,
            5);

        cruces.AddRange(crucesB5aB8);

        _partidoServicios.BloquearEdicionFase(Fase.Grupos);
        _auditoria.RegistrarSorteoCruces();
        
        return cruces;
    }

    public List<CruceDTO> GenerarOctavosDeFinal(List<CruceDTO> crucesDieciseisavos)
    {
        var octavos = new List<CruceDTO>();

        for (int i = 1; i <= 8; i++)
        {
            var cruceA = crucesDieciseisavos.First(c => c.Codigo == "A" + i);
            var cruceB = crucesDieciseisavos.First(c => c.Codigo == "B" + i);

            var cruce = new CruceDTO
            {
                Codigo = "C" + i,
                Fase = Fase.Octavos,
                ReferenciaLocal = "Ganador " + cruceA.Codigo,
                ReferenciaVisitante = "Ganador " + cruceB.Codigo
            };

            octavos.Add(cruce);
        }

        return octavos;
    }

    public List<CruceDTO> GenerarCuartosDeFinal(List<CruceDTO> octavos)
    {
        var cuartos = new List<CruceDTO>();

        for (int i = 1; i <= 4; i++)
        {
            int numeroCruceLocal = (i * 2) - 1;
            int numeroCruceVisitante = i * 2;

            var cruceLocal = octavos.First(c => c.Codigo == "C" + numeroCruceLocal);
            var cruceVisitante = octavos.First(c => c.Codigo == "C" + numeroCruceVisitante);

            var cruce = new CruceDTO
            {
                Codigo = "D" + i,
                Fase = Fase.Cuartos,
                ReferenciaLocal = "Ganador " + cruceLocal.Codigo,
                ReferenciaVisitante = "Ganador " + cruceVisitante.Codigo
            };

            cuartos.Add(cruce);
        }

        return cuartos;
    }

    public List<CruceDTO> GenerarSemifinales(List<CruceDTO> cuartos)
    {
        var semifinales = new List<CruceDTO>();

        var d1 = cuartos.First(c => c.Codigo == "D1");
        var d2 = cuartos.First(c => c.Codigo == "D2");
        var d3 = cuartos.First(c => c.Codigo == "D3");
        var d4 = cuartos.First(c => c.Codigo == "D4");

        semifinales.Add(new CruceDTO
        {
            Codigo = "S1",
            Fase = Fase.Semifinal,
            ReferenciaLocal = "Ganador " + d1.Codigo,
            ReferenciaVisitante = "Ganador " + d2.Codigo
        });

        semifinales.Add(new CruceDTO
        {
            Codigo = "S2",
            Fase = Fase.Semifinal,
            ReferenciaLocal = "Ganador " + d3.Codigo,
            ReferenciaVisitante = "Ganador " + d4.Codigo
        });

        return semifinales;
    }

    public List<CruceDTO> GenerarTercerPuestoYFinal(List<CruceDTO> semifinales)
    {
        var partidosFinales = new List<CruceDTO>();

        var s1 = semifinales.First(c => c.Codigo == "S1");
        var s2 = semifinales.First(c => c.Codigo == "S2");

        partidosFinales.Add(new CruceDTO
        {
            Codigo = "TercerPuesto",
            Fase = Fase.Tercero,
            ReferenciaLocal = "Perdedor " + s1.Codigo,
            ReferenciaVisitante = "Perdedor " + s2.Codigo
        });

        partidosFinales.Add(new CruceDTO
        {
            Codigo = "Final",
            Fase = Fase.Final,
            ReferenciaLocal = "Ganador " + s1.Codigo,
            ReferenciaVisitante = "Ganador " + s2.Codigo
        });

        return partidosFinales;
    }
    public EquipoDTO ObtenerCampeon(PartidoDTO partidoFinal)
    {
        if (partidoFinal == null)
        {
            throw new ArgumentException("La final no puede ser nula");
        }

        if (partidoFinal.fase != Fase.Final)
        {
            throw new ArgumentException("El partido debe ser la final");
        }

        if (partidoFinal.estadoPartido != EstadoPartido.Jugado)
        {
            throw new ArgumentException("La final debe estar jugada");
        }

        if (partidoFinal.golesLocal > partidoFinal.golesVisitante)
        {
            return partidoFinal.equipoLocal;
        }

        if (partidoFinal.golesVisitante > partidoFinal.golesLocal)
        {
            return partidoFinal.equipoVisitante;
        }

        throw new ArgumentException("La final no puede terminar empatada");
    }
    public CuadroSegundaFaseDTO GenerarCuadroSegundaFase(int semillaCrucesFase)
    {
        var clasificados = ObtenerClasificados(semillaCrucesFase);

        var dieciseisavos = GenerarCrucesFase(clasificados, semillaCrucesFase);

        AgregarPartidosDieciseisavos(dieciseisavos);

        var octavos = GenerarOctavosDeFinal(dieciseisavos);
        var cuartos = GenerarCuartosDeFinal(octavos);
        var semifinales = GenerarSemifinales(cuartos);
        var partidosFinales = GenerarTercerPuestoYFinal(semifinales);

        return new CuadroSegundaFaseDTO
        {
            Dieciseisavos = dieciseisavos,
            Octavos = octavos,
            Cuartos = cuartos,
            Semifinales = semifinales,
            PartidosFinales = partidosFinales
        };
    }
    
    private void AgregarPartidosDieciseisavos(List<CruceDTO> dieciseisavos)
{
    var partidos = _partidoServicios.ObtenerPartidos();

    if (partidos.Any(p => p.fase == Fase.Dieciseisavos))
    {
        return;
    }

    var estadios = ObtenerEstadiosDisponibles(partidos);
    var fechaInicio = ObtenerFechaInicioDieciseisavos(partidos);

    for (int i = 0; i < dieciseisavos.Count; i++)
    {
        var cruce = dieciseisavos[i];

        if (string.IsNullOrWhiteSpace(cruce.EquipoLocal.Equipo.nombre) ||
            string.IsNullOrWhiteSpace(cruce.EquipoVisitante.Equipo.nombre))
        {
            continue;
        }

        var estadio = estadios[i % estadios.Count];

        var partido = new PartidoDTO
        {
            Grupo = cruce.Codigo,
            Fecha = fechaInicio.AddHours(i * 4),
            Estadio = estadio,
            equipoLocal = cruce.EquipoLocal.Equipo,
            equipoVisitante = cruce.EquipoVisitante.Equipo,
            fase = Fase.Dieciseisavos,
            estadoPartido = EstadoPartido.Pendiente,
            golesLocal = 0,
            golesVisitante = 0
        };

        _partidoServicios.AgregarPartido(
            partido,
            partido.equipoLocal,
            partido.equipoVisitante,
            partido.Estadio);
    }
}

private List<EstadioDTO> ObtenerEstadiosDisponibles(List<PartidoDTO> partidos)
{
    var estadios = new List<EstadioDTO>();

    foreach (var partido in partidos)
    {
        if (!estadios.Any(e => e.Nombre == partido.Estadio.Nombre))
        {
            estadios.Add(partido.Estadio);
        }
    }

    if (estadios.Count == 0)
    {
        throw new ArgumentException("No se pueden generar partidos de dieciseisavos sin estadios disponibles.");
    }

    return estadios;
}

private DateTime ObtenerFechaInicioDieciseisavos(List<PartidoDTO> partidos)
{
    var partidosGrupos = partidos
        .Where(p => p.fase == Fase.Grupos)
        .ToList();

    if (partidosGrupos.Count == 0)
    {
        throw new ArgumentException("No se pueden generar partidos de dieciseisavos sin partidos de grupos.");
    }

    var ultimaFechaGrupos = partidosGrupos
        .OrderByDescending(p => p.Fecha)
        .First()
        .Fecha;

    return ultimaFechaGrupos.Date.AddDays(3).AddHours(14);
}
}