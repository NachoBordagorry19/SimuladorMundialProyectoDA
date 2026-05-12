using Dominio.Enums;
using Repositorio;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class CrucesSegundaFaseServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatos;
    private PartidoRepositorio _partidoRepositorio;
    private IServicioPartido _partidoServicios;
    private CrucesSegundaFaseServicio _crucesServicio;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatos = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatos);
        _partidoServicios = new PartidoServicios(_partidoRepositorio);
        _crucesServicio = new CrucesSegundaFaseServicio(_partidoServicios);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_OrdenaEquiposPorPuntos()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 2, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoA, equipoD, estadio, 3, 0);

        AgregarPartido(equipoB, equipoC, estadio, 2, 0);
        AgregarPartido(equipoB, equipoD, estadio, 1, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo A", ranking[0].EquipoNombre);
        Assert.AreEqual(9, ranking[0].Puntos);

        Assert.AreEqual("Equipo B", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);

        Assert.AreEqual("Equipo C", ranking[2].EquipoNombre);
        Assert.AreEqual(3, ranking[2].Puntos);

        Assert.AreEqual("Equipo D", ranking[3].EquipoNombre);
        Assert.AreEqual(0, ranking[3].Puntos);
    }

    private EquipoDTO CrearEquipo(string nombre)
    {
        return new EquipoDTO
        {
            nombre = nombre,
            confederacion = Confederacion.UEFA,
            rankingFifa = 1500
        };
    }

    private EstadioDTO CrearEstadio()
    {
        return new EstadioDTO
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "Estadio Centenario",
            CapacidadLocativa = 60000
        };
    }

    private void AgregarPartido(
        EquipoDTO local,
        EquipoDTO visitante,
        EstadioDTO estadio,
        int golesLocal,
        int golesVisitante)
    {
        var partido = new PartidoDTO
        {
            Grupo = "A",
            Fecha = new DateTime(2026, 06, 01),
            Estadio = estadio,
            equipoLocal = local,
            equipoVisitante = visitante,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = golesLocal,
            golesVisitante = golesVisitante
        };

        _partidoServicios.AgregarPartido(partido, local, visitante, estadio);
    }
    
    [TestMethod]
    public void ObtenerRankingGrupo_SiEmpatanEnPuntos_OrdenaPorDiferenciaDeGoles()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 1, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoD, equipoA, estadio, 1, 0);

        AgregarPartido(equipoB, equipoC, estadio, 5, 0);
        AgregarPartido(equipoB, equipoD, estadio, 5, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo B", ranking[0].EquipoNombre);
        Assert.AreEqual(6, ranking[0].Puntos);
        Assert.AreEqual(9, ranking[0].Diferencia);

        Assert.AreEqual("Equipo A", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);
        Assert.AreEqual(1, ranking[1].Diferencia);
    }
    
    [TestMethod]
    public void ObtenerRankingGrupo_SiEmpatanEnPuntosYDiferencia_OrdenaPorGolesAFavor()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 0, 2);
        AgregarPartido(equipoA, equipoC, estadio, 2, 0);
        AgregarPartido(equipoA, equipoD, estadio, 2, 0);

        AgregarPartido(equipoB, equipoC, estadio, 4, 0);
        AgregarPartido(equipoB, equipoD, estadio, 0, 4);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo B", ranking[0].EquipoNombre);
        Assert.AreEqual(6, ranking[0].Puntos);
        Assert.AreEqual(2, ranking[0].Diferencia);
        Assert.AreEqual(6, ranking[0].GolesAFavor);

        Assert.AreEqual("Equipo A", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);
        Assert.AreEqual(2, ranking[1].Diferencia);
        Assert.AreEqual(4, ranking[1].GolesAFavor);
    }
    
    [TestMethod]
    public void ObtenerRankingGrupo_SiPersisteEmpate_UsaSorteoDeterministico()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 0, 0);
        AgregarPartido(equipoA, equipoC, estadio, 0, 0);
        AgregarPartido(equipoA, equipoD, estadio, 0, 0);

        AgregarPartido(equipoB, equipoC, estadio, 0, 0);
        AgregarPartido(equipoB, equipoD, estadio, 0, 0);

        AgregarPartido(equipoC, equipoD, estadio, 0, 0);

        int semillaCrucesFase = 123;
        int otraSemillaCrucesFase = 456;

        var rankingSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", semillaCrucesFase);
        var rankingMismaSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", semillaCrucesFase);
        var rankingOtraSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", otraSemillaCrucesFase);

        var ordenSemillaCrucesFase = rankingSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();
        var ordenMismaSemillaCrucesFase = rankingMismaSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();
        var ordenOtraSemillaCrucesFase = rankingOtraSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();

        CollectionAssert.AreEqual(ordenSemillaCrucesFase, ordenMismaSemillaCrucesFase);

        bool mismoOrdenConDistintaSemilla = true;

        for (int i = 0; i < ordenSemillaCrucesFase.Count; i++)
        {
            if (ordenSemillaCrucesFase[i] != ordenOtraSemillaCrucesFase[i])
            {
                mismoOrdenConDistintaSemilla = false;
            }
        }

        Assert.IsFalse(mismoOrdenConDistintaSemilla);
    }
    
    [TestMethod]
    public void ObtenerRankingGrupo_AsignaGrupoALasPosiciones()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 2, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoA, equipoD, estadio, 3, 0);

        AgregarPartido(equipoB, equipoC, estadio, 2, 0);
        AgregarPartido(equipoB, equipoD, estadio, 1, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        foreach (var posicion in ranking)
        {
            Assert.AreEqual("A", posicion.Grupo);
        }
    }
}