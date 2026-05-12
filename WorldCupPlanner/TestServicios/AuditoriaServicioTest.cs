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
        Assert.IsTrue(log.Contains("Alta Usuario"), "alta usuario no contenido");
        Assert.IsTrue(log.Contains(email), "email no contenido");
        string fechaEsperada = DateTime.Now.ToString("yyyy-MM-dd");
        Assert.IsTrue(log.StartsWith(fechaEsperada));
    }
}