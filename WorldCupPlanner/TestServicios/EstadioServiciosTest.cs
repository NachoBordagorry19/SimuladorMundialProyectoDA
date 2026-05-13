using Moq;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;
using Servicios.Clases;

namespace TestServicios;

[TestClass]
public class EstadioServiciosTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IEstadioRepositorio _estadioRepositorio;
    private IServicioEstadio _servicioEstadio;
    private EstadioDTO _estadioDTO;
    private Mock<IServicioAuditoria> _auditoriaMock;


    [TestInitialize]
    public void TestInitialize()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _estadioRepositorio = new EstadioRepositorio(_baseDeDatosEnMemoria);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _servicioEstadio = new EstadioServicios(_estadioRepositorio, _auditoriaMock);

        _estadioDTO = new EstadioDTO()
        {
            Nombre = "CDS",
            Ciudad = "Montevideo",
            Descripcion = "descripcion",
            CapacidadLocativa = 50000,
        };
    }

    [TestMethod]
    public void AgregarEstadio_NombreUnicoEnSistema()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);
        Assert.AreEqual("CDS", _estadioDTO.Nombre);
        Assert.AreEqual("Montevideo", _estadioDTO.Ciudad);
        Assert.AreEqual("descripcion", _estadioDTO.Descripcion);
        Assert.AreEqual(50000, _estadioDTO.CapacidadLocativa);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEstadio_SiNombreExiste_LanzaExcepcion()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);
        _servicioEstadio.AgregarEstadio(_estadioDTO);
    }

    [TestMethod]
    public void ObtenerEstadio_DevuelveCorrectamente()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);

        EstadioDTO estadio2 = new EstadioDTO()
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "descripcion",
            CapacidadLocativa = 50000,
        };

        _servicioEstadio.AgregarEstadio(estadio2);

        var estadios = _servicioEstadio.ObtenerEstadios();
        Assert.AreEqual(2, estadios.Count);
    }

    [TestMethod]
    public void ObtenerEstadioPorNombre_DevuelveCorrectamente()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);

        EstadioDTO estadio2 = new EstadioDTO()
        {
            Nombre = "Monumental",
            Ciudad = "Ciudad",
            Descripcion = "descripcion",
            CapacidadLocativa = 30000
        };

        _servicioEstadio.AgregarEstadio(estadio2);

        EstadioDTO obtenido = _servicioEstadio.ObtenerEstadioPorNombre("Monumental");

        Assert.AreEqual("Monumental", obtenido.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerEstadioPorNombre_SiNoExiste_LanzaExcepcion()
    {
        _servicioEstadio.ObtenerEstadioPorNombre("NoExiste");
    }

    [TestMethod]
    public void EliminarEstadio_DevuelveCorrectamente()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);
        _servicioEstadio.EliminarEstadio(_estadioDTO);
        var estadios = _servicioEstadio.ObtenerEstadios();
        Assert.AreEqual(0, estadios.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarEstadio_SiNoExiste_LanzaExcepcion()
    {
        _servicioEstadio.EliminarEstadio(_estadioDTO);
    }

    [TestMethod]
    public void ActualizarEstadio_ModificaDatosCorrectamente()
    {
        _servicioEstadio.AgregarEstadio(_estadioDTO);

        EstadioDTO estadioActualizado = new EstadioDTO()
        {
            Nombre = "CDS",
            Ciudad = "Nueva Ciudad",
            Descripcion = "Nueva Descripcion",
            CapacidadLocativa = 60000
        };

        _servicioEstadio.ActualizarEstadio(estadioActualizado);
        EstadioDTO obtenido = _servicioEstadio.ObtenerEstadioPorNombre("CDS");

        Assert.AreEqual("Nueva Ciudad", obtenido.Ciudad);
        Assert.AreEqual("Nueva Descripcion", obtenido.Descripcion);
        Assert.AreEqual(60000, obtenido.CapacidadLocativa);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarEstadio_SiNoExiste_LanzaExcepcion()
    {
        EstadioDTO estadio = new EstadioDTO()
        {
            Nombre = "NoExiste",
            Ciudad = "Ciudad",
            Descripcion = "descripcion",
            CapacidadLocativa = 30000
        };

        _servicioEstadio.ActualizarEstadio(estadio);
    }
}