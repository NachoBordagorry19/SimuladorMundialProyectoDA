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

    [TestInitialize]
    public void TestInitialize()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _estadioRepositorio = new EstadioRepositorio(_baseDeDatosEnMemoria);
        _servicioEstadio = new EstadioServicios(_estadioRepositorio);

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
            Nombre = "CDS",
            Ciudad = "Montevideo",
            Descripcion = "descripcion",
            CapacidadLocativa = 50000,
        };
        
        _servicioEstadio.AgregarEstadio(estadio2);
        
        var estadios = _servicioEstadio.ObtenerEstadios();
        Assert.AreEqual(2, estadios.Count);
    }
}