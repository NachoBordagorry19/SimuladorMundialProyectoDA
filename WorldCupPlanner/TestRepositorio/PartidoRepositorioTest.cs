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
    
    
    
    
}