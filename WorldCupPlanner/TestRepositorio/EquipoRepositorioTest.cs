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
        Assert.AreEqual(0, equipos.Count);
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
    public void ObtenerEquipo_SiBuscoPorNombre_DevuelveCoincidencia()
    {
        var equipo1 = CrearEquipo("Peñarol");
        var equipo2 = CrearEquipo("Manya");

        _repositorioEquipo.AgregarEquipo(equipo1);
        _repositorioEquipo.AgregarEquipo(equipo2);

        var encontrado = _repositorioEquipo.ObtenerEquipo(e => e.Nombre == "Manya");

        Assert.IsNotNull(encontrado);
        Assert.AreEqual("Manya", encontrado.Nombre);
    }

    [TestMethod]
    public void BorrarEquipo_SiExiste_DejoListaVacia()
    {
        var equipo = CrearEquipo();
        _repositorioEquipo.AgregarEquipo(equipo);
        Assert.AreEqual(1, _repositorioEquipo.ObtenerEquipos().Count);
        _repositorioEquipo.EliminarEquipo(equipo);
        Assert.AreEqual(0, _repositorioEquipo.ObtenerEquipos().Count);
        var encontrado = _repositorioEquipo.ObtenerEquipo(e => e.Nombre == equipo.Nombre);
        Assert.IsNull(encontrado);
    }

    [TestMethod]
    public void ActualizarEquipo_BuscoPorNombre()
    {
        var equipo = CrearEquipo("Peñarol", Confederacion.CONMEBOL, 1000);
        _repositorioEquipo.AgregarEquipo(equipo);
        equipo.RankingFifa = 2000;

        _repositorioEquipo.ActualizarEquipo(equipo);

        var actualizado = _repositorioEquipo.ObtenerEquipo(e => e.Nombre == "Peñarol");
        Assert.IsNotNull(actualizado);
        Assert.AreEqual(2000, actualizado.RankingFifa);
    }
}