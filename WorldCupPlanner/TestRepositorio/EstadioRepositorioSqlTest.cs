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
}