namespace Servicios.Modelo;

public class NotificacionDTO
{
    public int Id { get; set; }
    public string Mensaje { get; set; } = String.Empty;
    public DateTime FechaHora { get; set; }
    public int UsuarioId { get; set; }
    public bool Leida { get; set; }
    public string? EtiquetaGrupo { get; set; }
}