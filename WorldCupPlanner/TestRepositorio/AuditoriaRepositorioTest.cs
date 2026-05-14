using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class AuditoriaRepositorioTest
{
    private AuditoriaRepositorio _auditoriaRepositorio;

    [TestInitialize]
    public void Inicializar()
    {
        _auditoriaRepositorio = new AuditoriaRepositorio();
    }

    [TestMethod]
    public void ObtenerTodosLosRegistros_SiNoHayRegistros_RetornaListaVacia()
    {
        var registros = _auditoriaRepositorio.ObtenerTodosLosRegistros();

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

        _auditoriaRepositorio.AgregarRegistro(registro);

        var registros = _auditoriaRepositorio.ObtenerTodosLosRegistros();

        Assert.AreEqual(1, registros.Count);
        Assert.AreEqual("admin@gmail.com", registros[0].Usuario);
        Assert.AreEqual("Alta usuario", registros[0].Accion);
    }

    [TestMethod]
    public void ObtenerTodosLosRegistros_RetornaCopiaDeLaLista()
    {
        _auditoriaRepositorio.AgregarRegistro(new Auditoria
        {
            Usuario = "admin@gmail.com",
            Accion = "Alta usuario",
            Detalle = "Se agregó un usuario"
        });

        var registros = _auditoriaRepositorio.ObtenerTodosLosRegistros();
        registros.Clear();

        var registrosOriginales = _auditoriaRepositorio.ObtenerTodosLosRegistros();

        Assert.AreEqual(1, registrosOriginales.Count);
    }
}