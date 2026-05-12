using Repositorio.Interfaces;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class ServicioAuditoria : IServicioAuditoria
{
    private readonly ISessionService _sessionService;
    private readonly IAuditoriaRepositorio _auditoriaRepo;
    public ServicioAuditoria(ISessionService sessionService, IAuditoriaRepositorio auditoriaRepo)
    {
        _sessionService = sessionService;
        _auditoriaRepo = auditoriaRepo;
    }
    public void RegistrarAltaUsuario(string email, string roles)
    {
        throw new NotImplementedException();
    }

    public List<string> ObtenerRegistrosFormateados()
    {
        throw new NotImplementedException();
    }
}