namespace Dominio.Clases;

public class Auditoria
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public string Usuario { get; set; }
    public string Accion { get; set; }
    public string Detalle { get; set; }

    public Auditoria()
    {
        
    }
}