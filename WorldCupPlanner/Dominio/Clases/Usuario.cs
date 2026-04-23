namespace Dominio.Clases;

public class Usuario
{
    private  string _nombre;
    private  string _apellido;
    private  string _email;
    private  DateTime _fechaNacimiento;
    private string _contraseña;

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("Nombre no debe ser vacio");
            }

            _nombre = value;
        }
    }

    public string Apellido
    {
        get => _apellido;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("Apellido no debe ser vacio");
            }
            _apellido = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("Email no debe ser vacio");
            }
            _email = value;
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

    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        _fechaNacimiento = fechaNacimiento;
        _contraseña = contraseña;
    }
}