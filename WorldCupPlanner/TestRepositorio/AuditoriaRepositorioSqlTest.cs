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

    [TestMethod]
    public void ObtenerTodosLosRegistros_SiNoHayRegistros_RetornaListaVacia()
    {
        var registros = _auditoriaRepositorioSql.ObtenerTodosLosRegistros();
        Assert.AreEqual(0, registros.Count);
    }
    
    [TestMethod]
    public void AgregarRegistro_SiRegistroValido_LoGuarda()
    {
        var registro = new Auditoria
        {
            Usuario = "admin@gmail.com",
            Accion = "Alta usuario",
            Detalle = "Se agregó un usuario"
        };

        _auditoriaRepositorioSql.AgregarRegistro(registro);

        var registros = _auditoriaRepositorioSql.ObtenerTodosLosRegistros();

        Assert.AreEqual(1, registros.Count);
        Assert.AreEqual("admin@gmail.com", registros[0].Usuario);
        Assert.AreEqual("Alta usuario", registros[0].Accion);
    }
}
