using Dominio.Clases;
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
        var log = new Auditoria
        {
            Usuario = _sessionService.ObtenerUsuarioLogeado().Email,
            Accion = "Alta Usuario",
            Detalle = $"Email: {email}, Roles: {roles}"
        };
        _auditoriaRepo.AgregarRegistro(log);
    }

    public List<string> ObtenerRegistrosFormateados()
    {
        var logs = _auditoriaRepo.ObtenerTodosLosRegistros();
        List<string> lista = new List<string>();
        foreach (var log in logs)
        {
            string timestamp = log.FechaHora.ToString("yyyy-MM-ddTHH:mm:ss");
            lista.Add($"{timestamp} | {log.Usuario} | {log.Accion} | {log.Detalle}");
        }
        return lista;
    }
}