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

    [TestMethod]
    public void AgregarEquipo()
    {
        _equipoRepositorioSql.AgregarEquipo(_equipo);
        List<Equipo> equipos = _equipoRepositorioSql.ObtenerEquipos();
        Assert.AreEqual(1, equipos.Count);
    }

    [TestMethod]
    public void EliminarUsuario()
    {
        _equipoRepositorioSql.AgregarEquipo(_equipo);
        List<Equipo> equiposIniciales = _equipoRepositorioSql.ObtenerEquipos();
        _equipoRepositorioSql.EliminarEquipo(_equipo);
        List<Equipo> equiposFinales = _equipoRepositorioSql.ObtenerEquipos();
        Assert.AreNotEqual(equiposFinales.Count, equiposIniciales.Count);
    }
}