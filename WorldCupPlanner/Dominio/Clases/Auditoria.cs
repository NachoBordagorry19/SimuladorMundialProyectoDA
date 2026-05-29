namespace Dominio.Clases;

public class Auditoria
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    
    
    public Auditoria()
    {
        FechaHora = DateTime.Now;
    }
}