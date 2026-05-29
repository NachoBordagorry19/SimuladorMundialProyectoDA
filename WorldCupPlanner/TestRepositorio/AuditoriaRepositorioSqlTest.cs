using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;
[TestClass]
public class AuditoriaRepositorioSqlTest
{
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;
    private AuditoriaRepositorioSql _auditoriaRepositorioSql;

    [TestInitialize]
    public void SetUp()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _auditoriaRepositorioSql = new AuditoriaRepositorioSql(_contexto);
        _contexto.SaveChanges();
    }
}