namespace Dominio.Clases;

public class Notificacion
{
    public int Id { get; set; }
    private string _mensaje = string.Empty;
    private DateTime _fechaHora;
    private bool _leida;
    private int _usuarioId;
    public string? EtiquetaGrupo { get; set; }

    public string Mensaje
    {
        get => _mensaje;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El mensaje no puede ser vacio");
            }
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
        get => _leida;
        set => _leida = value;
    }

    public int UsuarioId
    {
        get => _usuarioId;
        set
        {
            _usuarioId = Convert.ToInt32(value);
        }
    }
    
    public bool EsVacio(string textoATestear)
    {
        bool vacio = true;
        if (!String.IsNullOrEmpty(textoATestear))
        {
            vacio = false;
        }
        return vacio;
    }

    public void MarcarComoLeida()
    {
        this.Leida = true;
    }

    public bool YaLeida()
    {
        return this.Leida;
    }

    public Notificacion()
    {
        
    }
    
    public Notificacion(string mensaje, DateTime fechaHora, int usuarioId)
    {
        Mensaje = mensaje;
        FechaHora = fechaHora;
        UsuarioId = usuarioId;
        Leida = false;
    }
}