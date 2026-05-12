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
}