using Dominio.Enums;

namespace Dominio.Clases;

public class Log
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Mensaje { get; set; }
    public string Usuario { get; set; }
    public TipoLog Tipo { get; set; }

    public Log()
    {
        
    }
    
    public void Validar()
    {
        if (string.IsNullOrEmpty(Mensaje))
        {
            throw new ArgumentException("El mensaje no puede estar vacío.");
        }

    }
}