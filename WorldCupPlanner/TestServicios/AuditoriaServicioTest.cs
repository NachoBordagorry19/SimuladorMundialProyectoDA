using Servicios.Clases;

namespace TestServicios;

[TestClass]
public class AuditoriaServicioTest
{
    private AuditoriaRepositorioMock _repoMock;
    private SessionServiceMock _sessionMock;
    private ServicioAuditoria _servicio;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new AuditoriaRepositorioMock();
        _sessionMock = new SessionServiceMock();
        _servicio = new ServicioAuditoria(_sessionMock, _repoMock);
    }

    [TestMethod]
    public void RegistrarAltaUsuario_DebeGuardarRegistroCorrectamente()
    {
        string email = "usuario@gmail.com";
        string rol = "Administrador";
        _servicio.RegistrarAltaUsuario(email, rol);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Alta Usuario"));
        Assert.IsTrue(log.Contains(email));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaEsperada = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaEsperada));
    }

    [TestMethod]
    public void RegistrarEdicionUsuario_DebeGuardarRegistroCorrectamente()
    {
        string email = "editado@gmail.com";
        _servicio.RegistrarEdicionUsuario(email);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Edición Usuario"));
        Assert.IsTrue(log.Contains(email));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarEliminacionUsuario_DebeGuardarRegistroCorrectamente()
    {
        string email = "borrado@gmail.com";
        _servicio.RegistrarEliminacionUsuario(email);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Eliminación Usuario"));
        Assert.IsTrue(log.Contains(email));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarAltaEquipo_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Uruguay";
        _servicio.RegistrarAltaEquipo(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Alta Equipo"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarEdicionEquipo_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Brasil";
        _servicio.RegistrarEdicionEquipo(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Edición Equipo"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarEliminacionEquipo_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Italia";
        _servicio.RegistrarEliminacionEquipo(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Eliminación Equipo"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarAltaEstadio_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Estadio Centenario";
        _servicio.RegistrarAltaEstadio(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Alta Estadio"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarEdicionEstadio_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Estadio Centenario";
        _servicio.RegistrarEdicionEstadio(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Edición Estadio"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarEliminacionEstadio_DebeGuardarRegistroCorrectamente()
    {
        string nombre = "Estadio Lusail";
        _servicio.RegistrarEliminacionEstadio(nombre);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Eliminación Estadio"));
        Assert.IsTrue(log.Contains(nombre));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarModificacionPartido_DebeGuardarRegistroCorrectamente()
    {
        string detalleModificacion = "Resultado: Uruguay 2 - Argentina 1";
        _servicio.RegistrarModificacionPartido(detalleModificacion);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Modificación de Partido"));
        Assert.IsTrue(log.Contains(detalleModificacion));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
        string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaHoy));
    }

    [TestMethod]
    public void RegistrarImportacionEquipos_Error_DebeGuardarRegistroCorrectamente()
    {
        string error = "Archivo con formato inválido en línea 5";
        _servicio.RegistrarImportacionEquipos(error, false);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.IsTrue(registros[0].Contains("Importación de Equipos (Error)"));
        Assert.IsTrue(registros[0].Contains(error));
    }

    [TestMethod]
    public void RegistrarImportacionEquipos_Exito_DebeGuardarRegistroCorrectamente()
    {
        string exito = "Archivo válido";
        _servicio.RegistrarImportacionEquipos(exito, true);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.IsTrue(registros[0].Contains("Importación de Equipos (Éxito)"));
        Assert.IsTrue(registros[0].Contains(exito));
    }

    [TestMethod]
    public void RegistrarGeneracionAutomaticaEquipos_DebeGuardarRegistroCorrectamente()
    {
        int cantidad = 32;
        _servicio.RegistrarGeneracionAutomaticaEquipos(cantidad);
        var registros = _servicio.ObtenerRegistrosFormateados();

        Assert.AreEqual(1, registros.Count);
        string log = registros[0];
        Assert.IsTrue(log.Contains("Generación automática de equipos"));
        Assert.IsTrue(log.Contains(cantidad.ToString()));
        Assert.IsTrue(log.Contains("admin@gmail.com"));
    }

    [TestMethod]
    public void RegistrarGeneracionFixture_DebeGuardarRegistroCorrectamente()
    {
        _servicio.RegistrarGeneracionFixture();
        var log = _servicio.ObtenerRegistrosFormateados()[0];
        Assert.IsTrue(log.Contains("Generación de fixture"));
    }

    [TestMethod]
    public void RegistrarSorteoCruces_DebeGuardarRegistroCorrectamente()
    {
        _servicio.RegistrarSorteoCruces();
        var log = _servicio.ObtenerRegistrosFormateados()[0];
        Assert.IsTrue(log.Contains("Realización de sorteo para cruces"));
    }

    [TestMethod]
    public void RegistrarGeneracionNotificaciones_DebeGuardarRegistroCorrectamente()
    {
        string usuario = "usuario";
        string mensaje = "mensaje";
        _servicio.RegistrarGeneracionNotificacion(usuario, mensaje);
        var logs = _servicio.ObtenerRegistrosFormateados();
        Assert.AreEqual(1, logs.Count);
        string log = logs[0];
        Assert.IsTrue(log.Contains(usuario));
        Assert.IsTrue(log.Contains(mensaje));
        Assert.IsTrue(log.Contains("Generación de Notificaciones"));
    
    [TestMethod]
    public void RegistrarSimulacion_DebeGuardarRegistroCorrectamente()
    {
        _servicio.RegistrarSimulacion(781);
        var log = _servicio.ObtenerRegistrosFormateados()[0];
        Assert.IsTrue(log.Contains("Generación de simulación completada correctamente"));
    }

    [TestMethod]
    public void RegistrarCalculosDeCambioDeRanking_DebeGuardarCorrectamente()
    {
        _servicio.RegistrarCalculosDeCambioDeRanking(300,315);
        var log = _servicio.ObtenerRegistrosFormateados()[0];
        Assert.IsTrue(log.Contains("Cambio de ranking completado correctamente"));
    }
}