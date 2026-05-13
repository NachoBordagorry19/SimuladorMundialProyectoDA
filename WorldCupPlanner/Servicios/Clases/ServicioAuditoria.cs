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
    
    public void RegistrarEdicionUsuario(string email)
    {
        Registrar("Edición Usuario", $"Email: {email}");
    }
    
    public void RegistrarEliminacionUsuario(string email)
    {
        Registrar("Eliminación Usuario", $"Email: {email}");
    }
    
    public void RegistrarAltaEquipo(string nombre)
    {
        Registrar("Alta Equipo", $"Nombre: {nombre}");
    }
    
    public void RegistrarEdicionEquipo(string nombre)
    {
        Registrar("Edición Equipo", $"Nombre: {nombre}");
    }
    
    public void RegistrarEliminacionEquipo(string nombre)
    {
        Registrar("Eliminación Equipo", $"Nombre: {nombre}");
    }
    
    public void RegistrarAltaEstadio(string nombre)
    {
        Registrar("Alta Estadio", $"Nombre: {nombre}");
    }
    
    public void RegistrarEdicionEstadio(string nombre)
    {
        Registrar("Edición Estadio", $"Nombre: {nombre}");
    }
    
    public void RegistrarEliminacionEstadio(string nombre)
    {
        Registrar("Eliminación Estadio", $"Nombre: {nombre}");
    }
    
    public void RegistrarModificacionPartido(string detalle)
    {
        Registrar("Modificación de Partido", detalle);
    }
    
    public void RegistrarImportacionEquipos(string mensaje, bool esExito)
    {
        string etiqueta = esExito ? "Éxito" : "Error";
        Registrar($"Importación de Equipos ({etiqueta})", mensaje);
    }
    
    public void RegistrarGeneracionAutomaticaEquipos(int cantidad)
    {
        Registrar("Generación automática de equipos", $"Cantidad: {cantidad}");
    }
    
    public void RegistrarGeneracionFixture()
    {
        Registrar("Generación de fixture", "Proceso completado");
    }
    
    public void RegistrarSorteoCruces()
    {
        Registrar("Realización de sorteo para cruces", "Proceso completado");
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