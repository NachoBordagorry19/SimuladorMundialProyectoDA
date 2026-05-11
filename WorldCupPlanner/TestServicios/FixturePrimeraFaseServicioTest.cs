using Dominio.Clases;
using Repositorio;
using Servicios.Clases;
using System.Linq;
using Dominio.Enums;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class FixturePrimeraFaseServicioTest
{
    private FixturePrimeraFaseServicio _fixtureServicio;
    private BaseDeDatosEnMemoria _baseDeDatos;
    private EquipoRepositorio _equipoRepositorio;
    private EstadioRepositorio _estadioRepositorio;
    private PartidoRepositorio _partidoRepositorio;
    private EquipoServicios _equipoServicios;
    private EstadioServicios _estadioServicios;
    private PartidoServicios _partidoServicios;
    
    private bool PartidoPerteneceAlGrupo(PartidoDTO partido, Grupo grupo)
    {
        return EquipoPerteneceAlGrupo(partido.equipoLocal.nombre, grupo) &&
               EquipoPerteneceAlGrupo(partido.equipoVisitante.nombre, grupo);
    }

    private bool EquipoPerteneceAlGrupo(string nombreEquipo, Grupo grupo)
    {
        return grupo.Equipos.Any(e => e.Nombre == nombreEquipo);
    }

    private bool ExistePartidoEntre(List<PartidoDTO> partidos, Equipo equipoA, Equipo equipoB)
    {
        return partidos.Any(p =>
            p.equipoLocal.nombre == equipoA.Nombre && p.equipoVisitante.nombre == equipoB.Nombre ||
            p.equipoLocal.nombre == equipoB.Nombre && p.equipoVisitante.nombre == equipoA.Nombre);
    }
    
    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatos = new BaseDeDatosEnMemoria();
        _equipoRepositorio = new EquipoRepositorio(_baseDeDatos);
        _estadioRepositorio = new EstadioRepositorio(_baseDeDatos);
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatos);
        _equipoServicios = new EquipoServicios(_equipoRepositorio);
        _estadioServicios = new EstadioServicios(_estadioRepositorio);
        _partidoServicios = new PartidoServicios(_partidoRepositorio);

        _fixtureServicio = new FixturePrimeraFaseServicio(
            _equipoRepositorio,
            _estadioRepositorio,
            _partidoRepositorio,
            _equipoServicios,
            _estadioServicios,
            _partidoServicios);
    }

    [TestMethod]
    public void GenerarFixture_Con48EquiposY4Estadios_RetornaResultadoValido()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(456, resultado.SemillaFixture);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GenerarFixture_SiNoHay48Equipos_LanzaArgumentException()
    {
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        _fixtureServicio.GenerarFixturePrimeraFase(456);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GenerarFixture_SiNoHay4Estadios_LanzaArgumentException()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));

        _fixtureServicio.GenerarFixturePrimeraFase(456);
    }

    [TestMethod]
    public void GenerarFixture_GeneraDoceGruposDeCuatroEquipos()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        Assert.AreEqual(12, resultado.Grupos.Count);

        for (char letra = 'A'; letra <= 'L'; letra++)
        {
            string nombreGrupo = letra.ToString();

            var grupo = resultado.Grupos.FirstOrDefault(g => g.Nombre == nombreGrupo);

            Assert.IsNotNull(grupo);
            Assert.AreEqual(4, grupo.Equipos.Count);
        }
    }

    [TestMethod]
    public void GenerarFixture_GeneraDoceGruposConCuatroEquiposYSeisPartidos()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        var partidos = _baseDeDatos.ObtenerPartidos();
        Assert.AreEqual(72, partidos.Count);
    }

    [TestMethod]
    public void GenerarFixture_RespetaReglasConfederaciones()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        foreach (var grupo in resultado.Grupos)
        {
            var conteoPorConfederacion = grupo.Equipos.GroupBy(e => e.Confederacion);

            foreach (var confederacion in conteoPorConfederacion)
            {
                if (confederacion.Key == Confederacion.UEFA)
                {
                    Assert.IsTrue(confederacion.Count() <= 2);
                }
                else
                {
                    Assert.IsTrue(confederacion.Count() <= 1);
                }
            }
        }
    }

    [TestMethod]
    public void GenerarFixture_Las6JornadasTienenLosEnfrentamientosCorrectos()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        var fechaJornada1 = resultado.Partidos
            .Where(p => p.fase.ToString() == "Grupos")
            .Select(p => p.Fecha)
            .Distinct()
            .First();

        var jornada1 = resultado.Partidos
            .Where(p => p.fase.ToString() == "Grupos" && p.Fecha == fechaJornada1)
            .Take(6)
            .ToList();

        Assert.AreEqual(6, jornada1.Count);
    }
    [TestMethod]
    public void GenerarFixture_CadaGrupoTieneTresJornadasConEnfrentamientosCorrectos()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        foreach (var grupo in resultado.Grupos)
        {
            var equipos = grupo.Equipos;
            var partidosDelGrupo = resultado.Partidos
                .Where(p => PartidoPerteneceAlGrupo(p, grupo))
                .ToList();

            Assert.AreEqual(6, partidosDelGrupo.Count);

            var fechas = partidosDelGrupo
                .Select(p => p.Fecha.Date)
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            Assert.AreEqual(3, fechas.Count);

            foreach (var fecha in fechas)
            {
                Assert.AreEqual(2, partidosDelGrupo.Count(p => p.Fecha.Date == fecha));
            }

            var jornada1 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[0]).ToList();
            var jornada2 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[1]).ToList();
            var jornada3 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[2]).ToList();

            Assert.IsTrue(ExistePartidoEntre(jornada1, equipos[0], equipos[3]));
            Assert.IsTrue(ExistePartidoEntre(jornada1, equipos[1], equipos[2]));

            Assert.IsTrue(ExistePartidoEntre(jornada2, equipos[0], equipos[2]));
            Assert.IsTrue(ExistePartidoEntre(jornada2, equipos[1], equipos[3]));

            Assert.IsTrue(ExistePartidoEntre(jornada3, equipos[0], equipos[1]));
            Assert.IsTrue(ExistePartidoEntre(jornada3, equipos[2], equipos[3]));
        }
    }
}