using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Servicios.Clases;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class CrucesSegundaFaseServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatos;
    private PartidoRepositorio _partidoRepositorio;
    private PartidoServicios _partidoServicios;
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
}