using Dominio.Enums;

namespace Servicios.Modelo;

public class LogDTO
{
    public string fechaISO8601 { get; set; }
    public string mensaje { get; set; }
    public string usuario { get; set; }
    public TipoLog tipo { get; set; }

    public LogDTO()
    {
        
    }
}