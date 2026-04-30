namespace Servicios.Modelo;

public class EntradaAuditoria
{
    public int RankingFifa { get; set; }
    
    public List<string> OrdenOriginal { get; set; } = new List<string>();
    
    public List<string> OrdenResuelto { get; set; } = new List<string>();
    
    public int SemillaUsada { get; set; }
    
    public string Nota { get; set; } = string.Empty;
}