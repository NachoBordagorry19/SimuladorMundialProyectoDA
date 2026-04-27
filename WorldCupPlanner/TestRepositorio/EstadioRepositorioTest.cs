using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class EstadioRepositorioTest
{
    private BaseDeDatosEnMemoria BDEnMemoria = null;
    private EstadioRepositorio _repositorioEstadio = null;

    [TestInitialize]
    public void Inicializar()
    {
        BDEnMemoria = new BaseDeDatosEnMemoria();
        _repositorioEstadio = new EstadioRepositorio(BDEnMemoria);
    }

    private static Estadio CrearEstadio(
        string nombre = "Centenario",
        string ciudad = "Montevideo",
        string descripcion = "descripcion",
        int? capacidadLocativa = 50000)
    {
        return new Estadio(nombre, ciudad, descripcion, capacidadLocativa);
    }

    [TestMethod]
    public void ObtenerEstadios_CuandoNoHayEstadios_RetornaListaVacia()
    {
        var estadiosPrueba = _repositorioEstadio.ObtenerEstadios();
        Assert.IsNotNull(estadiosPrueba);
        Assert.AreEqual(0, estadiosPrueba.Count);
    }

    [TestMethod]
    public void AgregarEstadio_CuandoSeAgregaUno_ApareceEnLaLista()
    {
        var estadio = CrearEstadio();
        _repositorioEstadio.AgregarEstadio(estadio);
        var lista = _repositorioEstadio.ObtenerEstadios();
        Assert.AreEqual(1, lista.Count);
        Assert.AreEqual(estadio.Nombre, lista[0].Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarEstadio_NombreDuplicado_LanzaExcepcion()
    {
        var estadio1 = CrearEstadio("Centenario");
        var estadio2 = CrearEstadio("Centenario");
        _repositorioEstadio.AgregarEstadio(estadio1);
        _repositorioEstadio.AgregarEstadio(estadio2);
    }

    [TestMethod]
    public void ObtenerEstadio_SiBuscoPorNombre_DevuelveCoincidencia()
    {
        var estadio = CrearEstadio();
        var estadio2 = CrearEstadio("Gran Parque Central", "Montevideo", "descripcion", 30000);
        _repositorioEstadio.AgregarEstadio(estadio);
        _repositorioEstadio.AgregarEstadio(estadio2);
        var encontrado = _repositorioEstadio.ObtenerEstadioPorNombre("Gran Parque Central");
        Assert.IsNotNull(encontrado);
        Assert.AreSame(estadio2, encontrado);
    }

    [TestMethod]
    public void ObtenerEstadio_SiBuscoPorNombreInexistente_DevuelveNull()
    {
        var estadio = CrearEstadio();
        _repositorioEstadio.AgregarEstadio(estadio);
        var encontrado = _repositorioEstadio.ObtenerEstadioPorNombre("Nombre Inexistente");
        Assert.IsNull(encontrado);
    }

    [TestMethod]
    public void ActualizarEstadio_ModificaDatosCorrectamente()
    {
        var estadio = CrearEstadio();
        _repositorioEstadio.AgregarEstadio(estadio);
        estadio.Nombre = "Nuevo Nombre";
        estadio.Ciudad = "Nueva Ciudad";
        estadio.Descripcion = "Nueva Descripcion";
        estadio.CapacidadLocativa = 60000;
        var actualizado = _repositorioEstadio.ObtenerEstadioPorNombre("Nuevo Nombre");
        Assert.IsNotNull(actualizado);
        Assert.AreEqual("Nuevo Nombre", actualizado.Nombre);
        Assert.AreEqual("Nueva Ciudad", actualizado.Ciudad);
        Assert.AreEqual("Nueva Descripcion", actualizado.Descripcion);
        Assert.AreEqual(60000, actualizado.CapacidadLocativa);
    }

    [TestMethod]
    public void BorrarUsuario_SiUsuarioExiste_DejoListaVacia()
    {
        var estadio = CrearEstadio();
        _repositorioEstadio.AgregarEstadio(estadio);
        Assert.AreEqual(1, _repositorioEstadio.ObtenerEstadios().Count);
        _repositorioEstadio.EliminarEstadio(estadio);
        Assert.AreEqual(0, _repositorioEstadio.ObtenerEstadios().Count);
        var estadioBorrado = _repositorioEstadio.ObtenerEstadioPorNombre(estadio.Nombre);
        Assert.IsNull(estadioBorrado);
    }
}