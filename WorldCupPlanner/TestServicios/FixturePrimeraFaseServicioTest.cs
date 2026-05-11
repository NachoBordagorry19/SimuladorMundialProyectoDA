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

        var equiposOrdenados = resultado.EquiposOrdenados;

        var grupos = new List<List<EquipoDTO>>();
        for (int grupo = 0; grupo < 12; grupo++)
        {
            grupos.Add(new List<EquipoDTO>());
        }

        for (int bombo = 0; bombo < 4; bombo++)
        {
            for (int pos = 0; pos < 12; pos++)
            {
                grupos[pos].Add(equiposOrdenados[bombo * 12 + pos]);
            }
        }

        foreach (var grupo in grupos)
        {
            var conteoPorConf = grupo.GroupBy(e => e.confederacion);

            foreach (var confederacion in conteoPorConf)
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
    public void GenerarFixture_Las3JornadasTienenLosEnfrentamientosCorrectos()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_A", "Ciudad_A", "Descripcion A", 50000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_B", "Ciudad_B", "Descripcion B", 60000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_C", "Ciudad_C", "Descripcion C", 70000));
        _baseDeDatos.AgregarEstadio(new Estadio("Estadio_D", "Ciudad_D", "Descripcion D", 80000));

        var resultado = _fixtureServicio.GenerarFixturePrimeraFase(456);
        
        var partidos = _baseDeDatos.ObtenerPartidos();
        Assert.AreEqual(72, partidos.Count);
        
        var fechas = partidos.Select(p => p.Fecha).Distinct().OrderBy(f => f).ToList();
        Assert.AreEqual(3, fechas.Count);
        
        foreach (var fecha in fechas)
        {
            var partidosEnFecha = partidos.Where(p => p.Fecha == fecha).ToList();
            Assert.AreEqual(12, partidosEnFecha.Count);
        }
    }
}