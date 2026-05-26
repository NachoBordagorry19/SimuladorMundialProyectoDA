using Dominio.Clases;
using Dominio.Enums;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class EquipoRepositorioSqlTest
{
    private EquipoRepositorioSql _equipoRepositorioSql;
    private Equipo _equipo;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _equipoRepositorioSql = new EquipoRepositorioSql(_contexto);
        _equipo = new Equipo("Nacional", Confederacion.UEFA, 400);
    }

    [TestMethod]
    public void ObtenerEquipos_SeObtienenCorrectamente()
    {
        List<Equipo> equipos = _equipoRepositorioSql.ObtenerEquipos();
        Assert.AreEqual(0, equipos.Count);
    }
}