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
        Assert.AreEqual(0,estadiosPrueba.Count);
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
}