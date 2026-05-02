using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;

namespace TestRepositorio;

[TestClass]
public class PartidoRepositorioTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria = null;
    private IPartidoRepositorio _partidoRepositorio = null;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatosEnMemoria);
    }

    private static Partido CrearPartido()
    {
        Equipo local = new Equipo("A", Confederacion.CONMEBOL, 1000);
        Equipo visitante = new Equipo("B", Confederacion.UEFA, 1100);
        Estadio estadio = new Estadio("Campeon del Siglo", "Montevideo", "Descripcion", 60000);

        return new Partido(DateTime.Now, estadio, local, visitante, Fase.Grupos, 0, 0);
    }
    
    [TestMethod]
    public void ObtenerPartidos_CuandoNoHayPartidos_RetornaListaVacia()
    {
        var partidos = _partidoRepositorio.ObtenerPartidos();

        Assert.IsNotNull(partidos);
        Assert.AreEqual(0, partidos.Count);
    }
    
    
    [TestMethod]
    public void CrearPartido_AgregarEnLista()
    {
        var partido = CrearPartido();

        _partidoRepositorio.AgregarPartido(partido);

        var partidos = _partidoRepositorio.ObtenerPartidos();

        Assert.AreEqual(1, partidos.Count);
        Assert.AreSame(partido, partidos[0]);
    }
    
    [TestMethod]
    public void ObtenerPartidoPorId_SiExiste_DevuelveCoincidencia()
    {
        var partido1 = CrearPartido();
        var partido2 = CrearPartido();

        _partidoRepositorio.AgregarPartido(partido1);
        _partidoRepositorio.AgregarPartido(partido2);

        var encontrado = _partidoRepositorio.ObtenerPartidoPorId(partido2.Id);

        Assert.IsNotNull(encontrado);
        Assert.AreSame(partido2, encontrado);
    }
    
    [TestMethod]
    public void ObtenerPartidoPorId_SiNoExiste_DevuelveNull()
    {
        var partido = CrearPartido();

        _partidoRepositorio.AgregarPartido(partido);

        var encontrado = _partidoRepositorio.ObtenerPartidoPorId(-1);

        Assert.IsNull(encontrado);
    }
    
    [TestMethod]
    public void EliminarPartido_SiExiste_DejaListaVacia()
    {
        var partido = CrearPartido();

        _partidoRepositorio.AgregarPartido(partido);

        Assert.AreEqual(1, _partidoRepositorio.ObtenerPartidos().Count);

        _partidoRepositorio.EliminarPartido(partido);

        Assert.AreEqual(0, _partidoRepositorio.ObtenerPartidos().Count);
    }
    
    
    [TestMethod]
    public void ActualizarPartido_ModificaDatosCorrectamente()
    {
        var partido = CrearPartido();
        _partidoRepositorio.AgregarPartido(partido);

        Estadio nuevoEstadio = new Estadio("Centenario", "Montevideo", "Descripcion", 60000);
        Partido partidoActualizado = new Partido(
            partido.Fecha.AddDays(1),
            nuevoEstadio,
            partido.Local,
            partido.Visitante,
            partido.Fase,
            2,
            1
        );

        partidoActualizado.Id = partido.Id;

        var actualizado = _partidoRepositorio.ActualizarPartido(partidoActualizado);
        var obtenido = _partidoRepositorio.ObtenerPartidoPorId(partido.Id);

        Assert.IsTrue(actualizado);
        Assert.IsNotNull(obtenido);
        Assert.AreEqual(nuevoEstadio, obtenido.Estadio);
        Assert.AreEqual(2, obtenido.GolesLocal);
        Assert.AreEqual(1, obtenido.GolesVisitante);
    }
}