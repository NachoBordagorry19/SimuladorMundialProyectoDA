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
    private void Registrar(string accion, string detalle)
    {
        var log = new Auditoria
        {
            Usuario = _sessionService.ObtenerUsuarioLogeado().Email,
            Accion = accion,
            Detalle = detalle,
            FechaHora = DateTime.Now
        };
        _auditoriaRepo.AgregarRegistro(log);
    }

    public void RegistrarAltaUsuario(string email, string roles)
    {
        Registrar("Alta Usuario", $"Email: {email}, Roles: {roles}");
    }
    
    public void RegistrarAltaEquipo(string nombre)
    {
        Registrar("Alta Equipo", $"Nombre: {nombre}");
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