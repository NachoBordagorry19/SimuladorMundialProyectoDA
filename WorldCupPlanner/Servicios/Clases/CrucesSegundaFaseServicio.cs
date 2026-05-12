using Dominio.Enums;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class CrucesSegundaFaseServicio
{
    private readonly IServicioPartido _partidoServicios;

    public CrucesSegundaFaseServicio(IServicioPartido partidoServicios)
    {
        _partidoServicios = partidoServicios;
    }

    public List<PosicionEquipoDTO> ObtenerRankingGrupo(string grupo, int semillaCrucesFase)
    {
        var partidos = _partidoServicios.ObtenerPartidos()
            .Where(p => p.Grupo == grupo && p.fase == Fase.Grupos)
            .ToList();

        var posiciones = new List<PosicionEquipoDTO>();

        foreach (var partido in partidos)
        {
            AgregarEquipoSiNoExiste(posiciones, partido.equipoLocal.nombre, grupo);
            AgregarEquipoSiNoExiste(posiciones, partido.equipoVisitante.nombre, grupo);

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

    private void AgregarEquipoSiNoExiste(List<PosicionEquipoDTO> posiciones, string nombreEquipo, string grupo)
    {
        if (posiciones.Any(p => p.EquipoNombre == nombreEquipo))
        {
            return;
        }

        posiciones.Add(new PosicionEquipoDTO
        {
            EquipoNombre = nombreEquipo,
            Grupo = grupo
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
}