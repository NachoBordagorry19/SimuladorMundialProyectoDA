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
}