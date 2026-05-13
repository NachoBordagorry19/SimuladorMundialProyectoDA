using Dominio.Clases;
using Repositorio;
using Servicios.Clases;
using System.Linq;
using Dominio.Enums;
using Moq;
using Servicios.Interfaces;
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
    private Mock<IServicioAuditoria> _auditoriaMock;

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
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _equipoServicios = new EquipoServicios(_equipoRepositorio, _auditoriaMock.Object);
        _estadioServicios = new EstadioServicios(_estadioRepositorio, _auditoriaMock.Object);
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
            var partidosDelGrupo = resultado.Partidos.Where(p => p.Grupo == grupo.Nombre).ToList();

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

    [TestMethod]
    public void GenerarFixture_AsignaEstadiosOrdenadosPorNombreNormalizado()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);

        _baseDeDatos.AgregarEstadio(new Estadio("Monumental", "Buenos Aires", "descrip", 84000));
        _baseDeDatos.AgregarEstadio(new Estadio("Campeón del Siglo", "Montevideo", "descrip2", 40000));
        _baseDeDatos.AgregarEstadio(new Estadio("Bombonera", "Buenos Aires", "bocaboca", 54000));
        _baseDeDatos.AgregarEstadio(new Estadio("Centenario", "Montevideo", "descrip", 60000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        Assert.AreEqual("Bombonera", resultado.Partidos[0].Estadio.Nombre);
        Assert.AreEqual("Campeón del Siglo", resultado.Partidos[1].Estadio.Nombre);
        Assert.AreEqual("Centenario", resultado.Partidos[2].Estadio.Nombre);
        Assert.AreEqual("Monumental", resultado.Partidos[3].Estadio.Nombre);
        Assert.AreEqual("Bombonera", resultado.Partidos[4].Estadio.Nombre);
    }

    [TestMethod]
    public void GenerarFixture_AsignaGrupoACadaPartido()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        foreach (var grupo in resultado.Grupos)
        {
            var partidosDelGrupo = resultado.Partidos
                .Where(p => p.Grupo == grupo.Nombre)
                .ToList();

            Assert.AreEqual(6, partidosDelGrupo.Count);

            foreach (var partido in partidosDelGrupo)
            {
                Assert.IsTrue(PartidoPerteneceAlGrupo(partido, grupo));
            }
        }
    }

    [TestMethod]
    public void GenerarFixture_RespetaHorariosDeJornadas()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        foreach (var grupo in resultado.Grupos)
        {
            var partidosDelGrupo = resultado.Partidos
                .Where(p => p.Grupo == grupo.Nombre)
                .OrderBy(p => p.Fecha)
                .ToList();

            var fechas = partidosDelGrupo
                .Select(p => p.Fecha.Date)
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            var jornada1 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[0]).OrderBy(p => p.Fecha).ToList();
            var jornada2 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[1]).OrderBy(p => p.Fecha).ToList();
            var jornada3 = partidosDelGrupo.Where(p => p.Fecha.Date == fechas[2]).OrderBy(p => p.Fecha).ToList();

            Assert.AreEqual(14, jornada1[0].Fecha.Hour);
            Assert.AreEqual(18, jornada1[1].Fecha.Hour);

            Assert.AreEqual(14, jornada2[0].Fecha.Hour);
            Assert.AreEqual(18, jornada2[1].Fecha.Hour);

            Assert.AreEqual(14, jornada3[0].Fecha.Hour);
            Assert.AreEqual(14, jornada3[1].Fecha.Hour);

            Assert.AreEqual(jornada3[0].Fecha, jornada3[1].Fecha);
        }
    }

    [TestMethod]
    public void GenerarFixture_NoSuperaTresPartidosPorDia()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        var fechas = resultado.Partidos
            .Select(p => p.Fecha.Date)
            .Distinct()
            .ToList();

        foreach (var fecha in fechas)
        {
            int cantidadPartidosEnElDia = resultado.Partidos.Count(p => p.Fecha.Date == fecha);

            Assert.IsTrue(cantidadPartidosEnElDia <= 3);
        }
    }

    [TestMethod]
    public void GenerarFixture_UsaFechaInicioDefaultYSeparaJornadasTresDias()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);

        var fechasGrupoA = resultado.Partidos
            .Where(p => p.Grupo == "A")
            .Select(p => p.Fecha.Date)
            .Distinct()
            .OrderBy(f => f)
            .ToList();

        Assert.AreEqual(new DateTime(2026, 06, 01), fechasGrupoA[0]);
        Assert.AreEqual(new DateTime(2026, 06, 04), fechasGrupoA[1]);
        Assert.AreEqual(new DateTime(2026, 06, 07), fechasGrupoA[2]);
    }

    [TestMethod]
    public void GenerarFixture_UsaFechaInicioIndicada()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var fechaInicio = new DateTime(2026, 07, 10);

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456, fechaInicio);

        var primeraFechaGrupoA = resultado.Partidos
            .Where(p => p.Grupo == "A")
            .Select(p => p.Fecha.Date)
            .OrderBy(f => f)
            .First();

        Assert.AreEqual(fechaInicio, primeraFechaGrupoA);
    }
}