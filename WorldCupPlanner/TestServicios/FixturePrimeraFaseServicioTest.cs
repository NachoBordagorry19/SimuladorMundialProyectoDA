using Dominio.Clases;
using Repositorio;
using Servicios.Clases;

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
}