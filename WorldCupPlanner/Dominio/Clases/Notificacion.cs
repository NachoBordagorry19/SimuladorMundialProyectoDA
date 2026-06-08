namespace Dominio.Clases;

public class Notificacion
{
    public string _mensaje = string.Empty;
    public DateTime _fechaHora { get; set; }
    public bool _leida { get; set; } = false;
    public int _usuarioId { get; set; }

    public string Mensaje
    {
        get => _mensaje;
        set
        {
            _mensaje = value;
        }
    }
    
    public DateTime FechaHora
    {
        get => _fechaHora;
        set
        {
            _fechaHora = value;
        }
    }
    
    public bool Leida
    {
        set
        {
            _leida = value;
        }
    }

    public int UsuarioId
    {
        get => _usuarioId;
        set
        {
            _usuarioId = Convert.ToInt32(value);
        }
    }

    public Notificacion()
    {
        
    }
    
    public Notificacion(string mensaje, DateTime fechaHora, int usuarioId)
    {
        Mensaje = mensaje;
        FechaHora = fechaHora;
        Leida = false;
        UsuarioId = usuarioId;
    }
}