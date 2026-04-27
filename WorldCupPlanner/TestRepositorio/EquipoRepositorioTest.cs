using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using System.Linq;

namespace TestRepositorio;

[TestClass]
public class EquipoRepositorioTest
{
    private BaseDeDatosEnMemoria _BDenMemoria = null;
    private EquipoRepositorio _repositorioEquipo = null;
    
    [TestInitialize]
    public void Inicializar()
    {
        _BDenMemoria = new BaseDeDatosEnMemoria();
        _repositorioEquipo = new EquipoRepositorio(_BDenMemoria);
    }
    
    private static Equipo CrearEquipo(
        string nombre = "Uruguay",
        Confederacion confederacion = Confederacion.CONMEBOL,
        int ranking = 1000)
    {
        return new Equipo(nombre, confederacion, ranking);
    }
    
    [TestMethod]
    public void ObtenerEquipos_EsVacio()
    {
        var equipos = _repositorioEquipo.ObtenerEquipos();
        Assert.IsNotNull(equipos);
        Assert.AreEqual(0,equipos.Count);
    }
    
    [TestMethod]
    public void ObtenerEquipos_TieneUnEquipo()
    {
        var equipo = CrearEquipo();
        _repositorioEquipo.AgregarEquipo(equipo);
        var lista = _repositorioEquipo.ObtenerEquipos();
        Assert.AreEqual(1, lista.Count);
    }
    
    [TestMethod]
    public void ObtenerEquipo_ComparaPorFiltro()
    {
        var equipo1 = CrearEquipo("Uruguay");
        var equipo2 = CrearEquipo("Uruguay2");
        
        _repositorioEquipo.AgregarEquipo(equipo1);
        _repositorioEquipo.AgregarEquipo(equipo2);
        
        var encontrado = _repositorioEquipo.ObtenerEquipos(e => e.Nombre == "Uruguay2");
        Assert.IsNotNull(encontrado);
        Assert.AreEqual("Uruguay2", encontrado.Nombre);
    }
}