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
    private IServicioEstadio _estadioServicios;
    private EstadioDTO _estadioDTO;
    private Mock<IServicioAuditoria> _auditoriaMock;


    [TestInitialize]
    public void TestInitialize()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _estadioRepositorio = new EstadioRepositorio(_baseDeDatosEnMemoria);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _estadioServicios = new EstadioServicios(_estadioRepositorio, _auditoriaMock.Object);

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
        _estadioServicios.AgregarEstadio(_estadioDTO);
        Assert.AreEqual("CDS", _estadioDTO.Nombre);
        Assert.AreEqual("Montevideo", _estadioDTO.Ciudad);
        Assert.AreEqual("descripcion", _estadioDTO.Descripcion);
        Assert.AreEqual(50000, _estadioDTO.CapacidadLocativa);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEstadio_SiNombreExiste_LanzaExcepcion()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);
        _estadioServicios.AgregarEstadio(_estadioDTO);
    }
    
    [TestMethod]
    public void AgregarEstadio_DebeRegistrarAuditoria()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);
        _auditoriaMock.Verify(a => a.RegistrarAltaEstadio(_estadioDTO.Nombre), Times.Once);
    }

    [TestMethod]
    public void ObtenerEstadio_DevuelveCorrectamente()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);

        EstadioDTO estadio2 = new EstadioDTO()
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "descripcion",
            CapacidadLocativa = 50000,
        };

        _estadioServicios.AgregarEstadio(estadio2);

        var estadios = _estadioServicios.ObtenerEstadios();
        Assert.AreEqual(2, estadios.Count);
    }

    [TestMethod]
    public void ObtenerEstadioPorNombre_DevuelveCorrectamente()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);

        EstadioDTO estadio2 = new EstadioDTO()
        {
            Nombre = "Monumental",
            Ciudad = "Ciudad",
            Descripcion = "descripcion",
            CapacidadLocativa = 30000
        };

        _estadioServicios.AgregarEstadio(estadio2);

        EstadioDTO obtenido = _estadioServicios.ObtenerEstadioPorNombre("Monumental");

        Assert.AreEqual("Monumental", obtenido.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerEstadioPorNombre_SiNoExiste_LanzaExcepcion()
    {
        _estadioServicios.ObtenerEstadioPorNombre("NoExiste");
    }

    [TestMethod]
    public void EliminarEstadio_DevuelveCorrectamente()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);
        _estadioServicios.EliminarEstadio(_estadioDTO);
        var estadios = _estadioServicios.ObtenerEstadios();
        Assert.AreEqual(0, estadios.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarEstadio_SiNoExiste_LanzaExcepcion()
    {
        _estadioServicios.EliminarEstadio(_estadioDTO);
    }
    
    [TestMethod]
    public void EliminarEstadio_DebeRegistrarAuditoria()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);
        _estadioServicios.EliminarEstadio(_estadioDTO);
        
        _auditoriaMock.Verify(a => a.RegistrarEliminacionEstadio(_estadioDTO.Nombre), Times.Once);
    }

    [TestMethod]
    public void ActualizarEstadio_ModificaDatosCorrectamente()
    {
        _estadioServicios.AgregarEstadio(_estadioDTO);

        EstadioDTO estadioActualizado = new EstadioDTO()
        {
            Nombre = "CDS",
            Ciudad = "Nueva Ciudad",
            Descripcion = "Nueva Descripcion",
            CapacidadLocativa = 60000
        };

        _estadioServicios.ActualizarEstadio(estadioActualizado);
        EstadioDTO obtenido = _estadioServicios.ObtenerEstadioPorNombre("CDS");

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

        _estadioServicios.ActualizarEstadio(estadio);
    }
    
    [TestMethod]
    public void ActualizarEstadio_DebeRegistrarAuditoria()
    {
        var estadioInicial = new EstadioDTO { Nombre = "Lusail", Ciudad = "Lusail", CapacidadLocativa = 80000 };;
    
        _estadioServicios.AgregarEstadio(estadioInicial);

        var estadioEditado = new EstadioDTO { 
            Nombre = "Lusail", 
            Ciudad = "Lusail City", 
            CapacidadLocativa = 85000 
        };
        
        _estadioServicios.ActualizarEstadio(estadioEditado);
        _auditoriaMock.Verify(a => a.RegistrarEdicionEstadio(estadioEditado.Nombre), Times.Once);
    }
}