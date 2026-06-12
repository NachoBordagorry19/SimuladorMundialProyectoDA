using Dominio.Clases;
using Dominio.Enums;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class CrucesSegundaFaseServicio : IServicioCrucesSegundaFase
{
    private readonly IServicioPartido _partidoServicios;
    private readonly IServicioAuditoria _auditoria;

    public CrucesSegundaFaseServicio(IServicioPartido partidoServicios, IServicioAuditoria auditoria)
    {
        _partidoServicios = partidoServicios;
        _auditoria = auditoria;
    }

    public List<PosicionEquipoDTO> ObtenerRankingGrupo(string grupo, int semillaCrucesFase)
    {
        List<PartidoDTO> partidos = _partidoServicios.ObtenerPartidos()
            .Where(p => p.Grupo == grupo && p.Fase == Fase.Grupos)
            .ToList();

        var posiciones = new List<PosicionEquipoDTO>();

        foreach (var partido in partidos)
        {
            AgregarEquipoSiNoExiste(posiciones, partido.EquipoLocal, grupo);
            AgregarEquipoSiNoExiste(posiciones, partido.EquipoVisitante, grupo);

            if (partido.EstadoPartido != EstadoPartido.Jugado)
            {
                continue;
            }

            ProcesarResultadoPartido(posiciones, partido);
        }

        return OrdenarPosiciones(posiciones, semillaCrucesFase);
    }

    private void ProcesarResultadoPartido(List<PosicionEquipoDTO> posiciones, PartidoDTO partido)
    {
        PosicionEquipoDTO posicionLocal = BuscarPosicion(posiciones, partido.EquipoLocal.Nombre);
        PosicionEquipoDTO posicionVisitante = BuscarPosicion(posiciones, partido.EquipoVisitante.Nombre);

        posicionLocal.PartidosJugados++;
        posicionVisitante.PartidosJugados++;

        posicionLocal.GolesAFavor += partido.GolesLocal;
        posicionLocal.GolesEnContra += partido.GolesVisitante;

        posicionVisitante.GolesAFavor += partido.GolesVisitante;
        posicionVisitante.GolesEnContra += partido.GolesLocal;

        int puntosPorVictoria = 3;
        if (partido.GolesLocal > partido.GolesVisitante)
        {
            posicionLocal.Ganados++;
            posicionLocal.Puntos += puntosPorVictoria;

            posicionVisitante.Perdidos++;
        }
        else if (partido.GolesVisitante > partido.GolesLocal)
        {
            posicionVisitante.Ganados++;
            posicionVisitante.Puntos += puntosPorVictoria;

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

    private void AgregarEquipoSiNoExiste(List<PosicionEquipoDTO> posiciones, EquipoDTO equipo, string grupo)
    {
        if (posiciones.Any(p => p.EquipoNombre == equipo.Nombre))
        {
            return;
        }

        posiciones.Add(new PosicionEquipoDTO
        {
            EquipoNombre = equipo.Nombre,
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
            PosicionEquipoDTO posicionActual = posiciones[i];

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

            PosicionEquipoDTO auxiliar = equipos[i];
            equipos[i] = equipos[j];
            equipos[j] = auxiliar;
        }
    }

    public ClasificadosDTO ObtenerClasificados(int semillaCrucesFase)
    {
        var primeros = new List<PosicionEquipoDTO>();
        var segundos = new List<PosicionEquipoDTO>();
        var terceros = new List<PosicionEquipoDTO>();
        int partidosMinimosPorGrupo = 6;

        for (char letraGrupo = 'A'; letraGrupo <= 'L'; letraGrupo++)
        {
            string grupo = letraGrupo.ToString();

            int partidosJugados = _partidoServicios.ObtenerPartidos()
                .Count(p => p.Grupo == grupo &&
                            p.Fase == Fase.Grupos &&
                            p.EstadoPartido == EstadoPartido.Jugado);
            
            if (partidosJugados < partidosMinimosPorGrupo)
            {
                throw new ArgumentException(
                    "No se pueden generar cruces hasta que todos los grupos tengan sus partidos jugados.");
            }

            List<PosicionEquipoDTO> rankingGrupo = ObtenerRankingGrupo(grupo, semillaCrucesFase);

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
        List<PosicionEquipoDTO> posicionesOrdenadas = posiciones
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
        List<PosicionEquipoDTO> locales = equiposLocales.ToList();
        List<PosicionEquipoDTO> visitantes = equiposVisitantes.ToList();

        var random = new Random(semillaCrucesFase);

        MezclarFisherYates(locales, random);
        MezclarFisherYates(visitantes, random);

        var cruces = new List<CruceDTO>();

        for (int i = 0; i < locales.Count; i++)
        {
            PosicionEquipoDTO local = locales[i];
            int indiceVisitante = BuscarIndiceVisitanteDisponible(visitantes, local);

            PosicionEquipoDTO visitante = visitantes[indiceVisitante];
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
        List<PosicionEquipoDTO> mejoresPrimeros = clasificados.Primeros
            .Take(8)
            .ToList();

        List<PosicionEquipoDTO> terceros = clasificados.Terceros
            .ToList();

        List<CruceDTO> cruces = GenerarCrucesEntreListas(
            mejoresPrimeros,
            terceros,
            "A",
            semillaCrucesFase);

        cruces.AddRange(GenerarCrucesGrupoB(clasificados, semillaCrucesFase));

        _partidoServicios.BloquearEdicionFase(Fase.Grupos);
        _auditoria.RegistrarSorteoCruces();

        return cruces;
    }

    private List<CruceDTO> GenerarCrucesGrupoB(ClasificadosDTO clasificados, int semillaCrucesFase)
    {
        List<PosicionEquipoDTO> primerosRestantes = clasificados.Primeros
            .Skip(8)
            .Take(4)
            .ToList();

        List<PosicionEquipoDTO> segundosMenorPuntaje = clasificados.Segundos
            .OrderBy(s => s.Puntos)
            .Take(4)
            .ToList();

        List<CruceDTO> crucesB1aB4 = GenerarCrucesEntreListas(
            primerosRestantes,
            segundosMenorPuntaje,
            "B",
            semillaCrucesFase);

        List<PosicionEquipoDTO> segundosRestantes = clasificados.Segundos
            .Where(s => !segundosMenorPuntaje.Any(m => m.EquipoNombre == s.EquipoNombre))
            .ToList();

        List<PosicionEquipoDTO> segundosLocales = segundosRestantes
            .Take(4)
            .ToList();

        List<PosicionEquipoDTO> segundosVisitantes = segundosRestantes
            .Skip(4)
            .Take(4)
            .ToList();

        List<CruceDTO> crucesB5aB8 = GenerarCrucesEntreListas(
            segundosLocales,
            segundosVisitantes,
            "B",
            semillaCrucesFase,
            5);

        var cruces = new List<CruceDTO>();
        cruces.AddRange(crucesB1aB4);
        cruces.AddRange(crucesB5aB8);
        return cruces;
    }

    public List<CruceDTO> GenerarOctavosDeFinal(List<CruceDTO> crucesDieciseisavos)
    {
        var octavos = new List<CruceDTO>();

        for (int i = 1; i <= 8; i++)
        {
            CruceDTO cruceA = crucesDieciseisavos.First(c => c.Codigo == "A" + i);
            CruceDTO cruceB = crucesDieciseisavos.First(c => c.Codigo == "B" + i);

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

            CruceDTO cruceLocal = octavos.First(c => c.Codigo == "C" + numeroCruceLocal);
            CruceDTO cruceVisitante = octavos.First(c => c.Codigo == "C" + numeroCruceVisitante);

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

        CruceDTO d1 = cuartos.First(c => c.Codigo == "D1");
        CruceDTO d2 = cuartos.First(c => c.Codigo == "D2");
        CruceDTO d3 = cuartos.First(c => c.Codigo == "D3");
        CruceDTO d4 = cuartos.First(c => c.Codigo == "D4");

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

        CruceDTO s1 = semifinales.First(c => c.Codigo == "S1");
        CruceDTO s2 = semifinales.First(c => c.Codigo == "S2");

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

        if (partidoFinal.Fase != Fase.Final)
        {
            throw new ArgumentException("El partido debe ser la final");
        }

        if (partidoFinal.EstadoPartido != EstadoPartido.Jugado)
        {
            throw new ArgumentException("La final debe estar jugada");
        }

        if (partidoFinal.GolesLocal > partidoFinal.GolesVisitante)
        {
            return partidoFinal.EquipoLocal;
        }

        if (partidoFinal.GolesVisitante > partidoFinal.GolesLocal)
        {
            return partidoFinal.EquipoVisitante;
        }

        throw new ArgumentException("La final no puede terminar empatada");
    }

    public EquipoDTO? ObtenerCampeonActual()
    {
        List<PartidoDTO> partidos = _partidoServicios.ObtenerPartidos();

        PartidoDTO final = partidos.FirstOrDefault(p => p.Fase == Fase.Final);

        if (final == null)
        {
            return null;
        }

        if (final.EstadoPartido != EstadoPartido.Jugado)
        {
            return null;
        }

        return ObtenerCampeon(final);
    }

    public CuadroSegundaFaseDTO GenerarCuadroSegundaFase(int semillaCrucesFase)
    {
        var clasificados = ObtenerClasificados(semillaCrucesFase);

        List<CruceDTO> dieciseisavos = GenerarCrucesFase(clasificados, semillaCrucesFase);

        AgregarPartidosDieciseisavos(dieciseisavos);

        return ObtenerCuadroActual();
    }

    private void AgregarPartidosDieciseisavos(List<CruceDTO> dieciseisavos)
    {
        List<PartidoDTO> partidos = _partidoServicios.ObtenerPartidos();

        if (partidos.Any(p => p.Fase == Fase.Dieciseisavos))
        {
            return;
        }

        List<EstadioDTO> estadios = ObtenerEstadiosDisponibles(partidos);
        DateTime fechaInicio = ObtenerFechaInicioDieciseisavos(partidos);

        for (int i = 0; i < dieciseisavos.Count; i++)
        {
            var cruce = dieciseisavos[i];

            if (string.IsNullOrWhiteSpace(cruce.EquipoLocal.Equipo.Nombre) ||
                string.IsNullOrWhiteSpace(cruce.EquipoVisitante.Equipo.Nombre))
            {
                continue;
            }

            var estadio = estadios[i % estadios.Count];

            var partido = new PartidoDTO
            {
                Grupo = cruce.Codigo,
                Fecha = fechaInicio.AddHours(i * 4),
                Estadio = estadio,
                EquipoLocal = cruce.EquipoLocal.Equipo,
                EquipoVisitante = cruce.EquipoVisitante.Equipo,
                Fase = Fase.Dieciseisavos,
                EstadoPartido = EstadoPartido.Pendiente,
                GolesLocal = 0,
                GolesVisitante = 0
            };

            _partidoServicios.AgregarPartido(
                partido,
                partido.EquipoLocal,
                partido.EquipoVisitante,
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
        List<PartidoDTO> partidosGrupos = partidos
            .Where(p => p.Fase == Fase.Grupos)
            .ToList();

        if (partidosGrupos.Count == 0)
        {
            throw new ArgumentException("No se pueden generar partidos de dieciseisavos sin partidos de grupos.");
        }

        DateTime ultimaFechaGrupos = partidosGrupos
            .OrderByDescending(p => p.Fecha)
            .First()
            .Fecha;

        return ultimaFechaGrupos.Date.AddDays(3).AddHours(14);
    }

    public void ProcesarAvanceDelTorneo()
    {
        List<PartidoDTO> partidos = _partidoServicios.ObtenerPartidos();

        if (PuedeGenerarOctavos(partidos))
        {
            GenerarPartidosOctavos(partidos);
            return;
        }

        if (PuedeGenerarCuartos(partidos))
        {
            GenerarPartidosCuartos(partidos);
            return;
        }

        if (PuedeGenerarSemifinales(partidos))
        {
            GenerarPartidosSemifinales(partidos);
            return;
        }

        if (PuedeGenerarTercerPuestoYFinal(partidos))
        {
            GenerarPartidosTercerPuestoYFinal(partidos);
        }
    }
    private bool PuedeGenerarOctavos(List<PartidoDTO> partidos)
    {
        return FaseCompleta(partidos, Fase.Dieciseisavos, 16)
               && !partidos.Any(p => p.Fase == Fase.Octavos);
    }

    private bool PuedeGenerarCuartos(List<PartidoDTO> partidos)
    {
        return FaseCompleta(partidos, Fase.Octavos, 8)
               && !partidos.Any(p => p.Fase == Fase.Cuartos);
    }

    private bool PuedeGenerarSemifinales(List<PartidoDTO> partidos)
    {
        return FaseCompleta(partidos, Fase.Cuartos, 4)
               && !partidos.Any(p => p.Fase == Fase.Semifinal);
    }

    private bool PuedeGenerarTercerPuestoYFinal(List<PartidoDTO> partidos)
    {
        return FaseCompleta(partidos, Fase.Semifinal, 2)
               && !partidos.Any(p => p.Fase == Fase.Tercero)
               && !partidos.Any(p => p.Fase == Fase.Final);
    }

    private bool FaseCompleta(List<PartidoDTO> partidos, Fase fase, int cantidadEsperada)
    {
        List<PartidoDTO> partidosFase = partidos
            .Where(p => p.Fase == fase)
            .ToList();

        return partidosFase.Count == cantidadEsperada &&
               partidosFase.All(p => p.EstadoPartido == EstadoPartido.Jugado);
    }
    private void GenerarPartidosOctavos(List<PartidoDTO> partidos)
    {
        DateTime fechaInicio = ObtenerFechaInicioSiguienteFase(partidos, Fase.Dieciseisavos);
        List<EstadioDTO> estadios = ObtenerEstadiosDisponibles(partidos);

        for (int i = 1; i <= 8; i++)
        {
            PartidoDTO partidoA = ObtenerPartidoPorCodigo(partidos, Fase.Dieciseisavos, "A" + i);
            PartidoDTO partidoB = ObtenerPartidoPorCodigo(partidos, Fase.Dieciseisavos, "B" + i);

            AgregarPartidoEliminatorio(
                "C" + i,
                Fase.Octavos,
                ObtenerGanador(partidoA),
                ObtenerGanador(partidoB),
                fechaInicio.AddHours((i - 1) * 4),
                estadios[(i - 1) % estadios.Count]);
        }
    }
    private void GenerarPartidosCuartos(List<PartidoDTO> partidos)
    {
        DateTime fechaInicio = ObtenerFechaInicioSiguienteFase(partidos, Fase.Octavos);
        List<EstadioDTO> estadios = ObtenerEstadiosDisponibles(partidos);

        for (int i = 1; i <= 4; i++)
        {
            int numeroLocal = (i * 2) - 1;
            int numeroVisitante = i * 2;

            PartidoDTO partidoLocal = ObtenerPartidoPorCodigo(partidos, Fase.Octavos, "C" + numeroLocal);
            PartidoDTO partidoVisitante = ObtenerPartidoPorCodigo(partidos, Fase.Octavos, "C" + numeroVisitante);

            AgregarPartidoEliminatorio(
                "D" + i,
                Fase.Cuartos,
                ObtenerGanador(partidoLocal),
                ObtenerGanador(partidoVisitante),
                fechaInicio.AddHours((i - 1) * 4),
                estadios[(i - 1) % estadios.Count]);
        }
    }
    private void GenerarPartidosSemifinales(List<PartidoDTO> partidos)
    {
        DateTime fechaInicio = ObtenerFechaInicioSiguienteFase(partidos, Fase.Cuartos);
        List<EstadioDTO> estadios = ObtenerEstadiosDisponibles(partidos);

        PartidoDTO d1 = ObtenerPartidoPorCodigo(partidos, Fase.Cuartos, "D1");
        PartidoDTO d2 = ObtenerPartidoPorCodigo(partidos, Fase.Cuartos, "D2");
        PartidoDTO d3 = ObtenerPartidoPorCodigo(partidos, Fase.Cuartos, "D3");
        PartidoDTO d4 = ObtenerPartidoPorCodigo(partidos, Fase.Cuartos, "D4");

        AgregarPartidoEliminatorio(
            "S1",
            Fase.Semifinal,
            ObtenerGanador(d1),
            ObtenerGanador(d2),
            fechaInicio,
            estadios[0 % estadios.Count]);

        AgregarPartidoEliminatorio(
            "S2",
            Fase.Semifinal,
            ObtenerGanador(d3),
            ObtenerGanador(d4),
            fechaInicio.AddHours(4),
            estadios[1 % estadios.Count]);
    }
    private void GenerarPartidosTercerPuestoYFinal(List<PartidoDTO> partidos)
    {
        DateTime fechaInicio = ObtenerFechaInicioSiguienteFase(partidos, Fase.Semifinal);
        List<EstadioDTO> estadios = ObtenerEstadiosDisponibles(partidos);

        PartidoDTO s1 = ObtenerPartidoPorCodigo(partidos, Fase.Semifinal, "S1");
        PartidoDTO s2 = ObtenerPartidoPorCodigo(partidos, Fase.Semifinal, "S2");

        AgregarPartidoEliminatorio(
            "TercerPuesto",
            Fase.Tercero,
            ObtenerPerdedor(s1),
            ObtenerPerdedor(s2),
            fechaInicio,
            estadios[0 % estadios.Count]);

        AgregarPartidoEliminatorio(
            "Final",
            Fase.Final,
            ObtenerGanador(s1),
            ObtenerGanador(s2),
            fechaInicio.AddHours(4),
            estadios[1 % estadios.Count]);
    }
    private PartidoDTO ObtenerPartidoPorCodigo(List<PartidoDTO> partidos, Fase fase, string codigo)
    {
        return partidos.First(p => p.Fase == fase && p.Grupo == codigo);
    }

    private EquipoDTO ObtenerGanador(PartidoDTO partido)
    {
        if (partido.GolesLocal > partido.GolesVisitante)
        {
            return partido.EquipoLocal;
        }

        if (partido.GolesVisitante > partido.GolesLocal)
        {
            return partido.EquipoVisitante;
        }

        throw new ArgumentException("Un partido eliminatorio no puede terminar empatado.");
    }

    private EquipoDTO ObtenerPerdedor(PartidoDTO partido)
    {
        if (partido.GolesLocal > partido.GolesVisitante)
        {
            return partido.EquipoVisitante;
        }

        if (partido.GolesVisitante > partido.GolesLocal)
        {
            return partido.EquipoLocal;
        }

        throw new ArgumentException("Un partido eliminatorio no puede terminar empatado.");
    }

    private DateTime ObtenerFechaInicioSiguienteFase(List<PartidoDTO> partidos, Fase faseAnterior)
    {
        DateTime ultimaFecha = partidos
            .Where(p => p.Fase == faseAnterior)
            .OrderByDescending(p => p.Fecha)
            .First()
            .Fecha;

        return ultimaFecha.Date.AddDays(3).AddHours(14);
    }

    private void AgregarPartidoEliminatorio(
        string codigo,
        Fase fase,
        EquipoDTO local,
        EquipoDTO visitante,
        DateTime fecha,
        EstadioDTO estadio)
    {
        var partido = new PartidoDTO
        {
            Grupo = codigo,
            Fecha = fecha,
            Estadio = estadio,
            EquipoLocal = local,
            EquipoVisitante = visitante,
            Fase = fase,
            EstadoPartido = EstadoPartido.Pendiente,
            GolesLocal = 0,
            GolesVisitante = 0
        };

        _partidoServicios.AgregarPartido(
            partido,
            partido.EquipoLocal,
            partido.EquipoVisitante,
            partido.Estadio);
    }
    public CuadroSegundaFaseDTO ObtenerCuadroActual()
    {
        List<PartidoDTO> partidos = _partidoServicios.ObtenerPartidos();

        return new CuadroSegundaFaseDTO
        {
            Dieciseisavos = ConvertirPartidosACruces(partidos, Fase.Dieciseisavos),
            Octavos = ConvertirPartidosACruces(partidos, Fase.Octavos),
            Cuartos = ConvertirPartidosACruces(partidos, Fase.Cuartos),
            Semifinales = ConvertirPartidosACruces(partidos, Fase.Semifinal),
            PartidosFinales = ConvertirPartidosFinalesACruces(partidos)
        };
    }

    private List<CruceDTO> ConvertirPartidosACruces(List<PartidoDTO> partidos, Fase fase)
    {
        List<PartidoDTO> partidosFase = partidos
            .Where(p => p.Fase == fase)
            .OrderBy(p => p.Fecha)
            .ToList();

        var cruces = new List<CruceDTO>();

        foreach (var partido in partidosFase)
        {
            cruces.Add(PartidoACruce(partido));
        }

        return cruces;
    }

    private List<CruceDTO> ConvertirPartidosFinalesACruces(List<PartidoDTO> partidos)
    {
        List<PartidoDTO> partidosFinales = partidos
            .Where(p => p.Fase == Fase.Tercero || p.Fase == Fase.Final)
            .OrderBy(p => p.Fecha)
            .ToList();

        var cruces = new List<CruceDTO>();

        foreach (var partido in partidosFinales)
        {
            cruces.Add(PartidoACruce(partido));
        }

        return cruces;
    }

    private CruceDTO PartidoACruce(PartidoDTO partido)
    {
        return new CruceDTO
        {
            Codigo = partido.Grupo,
            Fase = partido.Fase,
            EquipoLocal = new PosicionEquipoDTO
            {
                EquipoNombre = partido.EquipoLocal.Nombre,
                Grupo = partido.Grupo,
                Equipo = partido.EquipoLocal
            },
            EquipoVisitante = new PosicionEquipoDTO
            {
                EquipoNombre = partido.EquipoVisitante.Nombre,
                Grupo = partido.Grupo,
                Equipo = partido.EquipoVisitante
            }
        };
    }
}