using Dominio.Clases;
using Dominio.Enums;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class IncidenciaRepositorioTest
{
    private IncidenciaRepositorio _incidenciaRepositorio;
    private Incidencia _incidencia;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Incidencias.RemoveRange(_contexto.Incidencias);
        _contexto.SaveChanges();
        _incidenciaRepositorio = new IncidenciaRepositorio(_contexto);
        _incidencia = new Incidencia(2, 2, TipoIncidencia.TarjetaAmarilla);
    }

    [TestMethod]
    public void ObtenerIncidencia_SeObtieneCorrectamente()
    {
        List<Incidencia> incidenciasPorPartido = _incidenciaRepositorio.ObtenerIncidencias(_incidencia._idPartido);
        Assert.AreEqual(0,incidenciasPorPartido.Count);
    }

    [TestMethod]
    public void AgregarIncidencia_SeAgregaCorrectamente()
    {
        _incidenciaRepositorio.AgregarIncidencia(_incidencia);
        List<Incidencia> incidenciasPorPartido = _incidenciaRepositorio.ObtenerIncidencias(_incidencia._idPartido);
        Assert.AreEqual(1,incidenciasPorPartido.Count);
    }

    [TestMethod]
    public void EliminarIncidencia_SeEliminaCorrectamente()
    {
        _incidenciaRepositorio.AgregarIncidencia(_incidencia);
        List<Incidencia> incidenciasPorPartidoIniciales = _incidenciaRepositorio.ObtenerIncidencias(_incidencia._idPartido);
        _incidenciaRepositorio.EliminarIncidencia(_incidencia.Id);
        List<Incidencia> incidenciasPorPartidoFinales = _incidenciaRepositorio.ObtenerIncidencias(_incidencia._idPartido);
        Assert.AreNotEqual(incidenciasPorPartidoIniciales.Count,incidenciasPorPartidoFinales.Count);
    }
}