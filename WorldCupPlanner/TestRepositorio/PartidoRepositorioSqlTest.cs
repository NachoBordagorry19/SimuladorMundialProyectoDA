using Dominio.Clases;
using Dominio.Enums;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class PartidoRepositorioSqlTest
{
    private PartidoRepositorioSql _partidoRepositorioSql;
    private Partido _partido;
    private Equipo _local;
    private Equipo _visitante;
    private Estadio _estadio;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Partidos.RemoveRange(_contexto.Partidos);
        _contexto.SaveChanges();

        _local = new Equipo("Argentina", Confederacion.CONMEBOL, 400);
        _visitante = new Equipo("Brasil", Confederacion.CONMEBOL, 500);
        _estadio = new Estadio("Monumental", "Buenos Aires", "Estadio de Argentina", 84567);

        _contexto.Equipos.Add(_local);
        _contexto.Equipos.Add(_visitante);
        _contexto.Estadios.Add(_estadio);
        _contexto.SaveChanges();

        _partidoRepositorioSql = new PartidoRepositorioSql(_contexto);
        _partido = new Partido(DateTime.Today, _estadio, _local, _visitante, Fase.Grupos, 0, 0);
    }

    [TestMethod]
    public void ObtenerPartidos_ListaVacia()
    {
        List<Partido> partidos = _partidoRepositorioSql.ObtenerPartidos();
        Assert.AreEqual(0, partidos.Count);
    }
}