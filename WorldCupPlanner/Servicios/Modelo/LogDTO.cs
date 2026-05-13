using Dominio.Enums;

namespace Servicios.Modelo;

public class LogDTO
{
    public string fechaISO8601 { get; set; } = string.Empty;
    public string mensaje { get; set; } = string.Empty;
    public string usuario { get; set; } = string.Empty;
    public TipoLog tipo { get; set; }

    public LogDTO()
    {

    }
}