using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class EstadioRepositorioSqlTest
{
    private EstadioRepositorioSql _estadioRepositorioSql;
    private Estadio _estadio;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Estadios.RemoveRange(_contexto.Estadios);
        _contexto.SaveChanges();
        _estadioRepositorioSql = new EstadioRepositorioSql(_contexto);
        _estadio = new Estadio("Allianz Arena", "Munich", "El mejor estadio del mundo", 75024);
    }

    [TestMethod]
    public void ObtenerEstadios_SeObtienenCorrectamente()
    {
        List<Estadio> estadios = _estadioRepositorioSql.ObtenerEstadios();
        Assert.AreEqual(0, estadios.Count);
    }

    [TestMethod]
    public void AgregarEstadio()
    {
        _estadioRepositorioSql.AgregarEstadio(_estadio);
        List<Estadio> estadios = _estadioRepositorioSql.ObtenerEstadios();
        Assert.AreEqual(1, estadios.Count);
    }

    [TestMethod]
    public void EliminarEstadio()
    {
        _estadioRepositorioSql.AgregarEstadio(_estadio);
        List<Estadio> estadiosIniciales = _estadioRepositorioSql.ObtenerEstadios();
        _estadioRepositorioSql.EliminarEstadio(_estadio);
        List<Estadio> estadiosFinales = _estadioRepositorioSql.ObtenerEstadios();
        Assert.AreNotEqual(estadiosFinales.Count, estadiosIniciales.Count);
    }

    [TestMethod]
    public void ObtenerEstadio_SeObtieneCorrectamente()
    {
        _estadioRepositorioSql.AgregarEstadio(_estadio);
        Estadio estadioPrueba = _estadioRepositorioSql.ObtenerEstadio(e => e.Nombre == "Allianz Arena");
        Assert.AreEqual(_estadio.Nombre, estadioPrueba.Nombre);
    }
}