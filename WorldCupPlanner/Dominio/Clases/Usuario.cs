using System;
using System.Linq;
using Dominio.Enums;

namespace Dominio.Clases;


public class Usuario
{
    private  string _nombre;
    private  string _apellido;
    private  string _email;
    private  DateTime _fechaNacimiento;
    private string _contraseña;
    private readonly List<Rol> _roles = new();

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

    public DateTime FechaNacimiento
    {
        get => _fechaNacimiento;
        set
        {
            if (value == DateTime.MinValue)
            {
                throw new ArgumentException("Fecha de nacimiento debe ser real");
            }
            if (value.Year < 1900)
            {
                throw new ArgumentException("Fecha de nacimiento debe ser posterior al año 1900");
            }
            if (value > DateTime.Today)
            {
                throw new ArgumentException("Fecha de nacimiento no puede ser en el futuro");
            }
            _fechaNacimiento = value;
        }
    }

    public string Contraseña
    {
        get => _contraseña;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("Contraseña no debe ser vacio");
            }
            else if (!value.Any(char.IsUpper))
            {
                throw new ArgumentException("Contraseña debe contener al menos una letra mayúscula");
            }
            else if (!value.Any(char.IsLower))
            {
                throw new ArgumentException("La contraseña debe contener una letra minusucla");
            }
            else if (!value.Any(char.IsDigit))
                throw new ArgumentException("Contraseña debe contener al menos un número");
            else if (!value.Any(c => !char.IsLetterOrDigit(c)))
                throw new ArgumentException("Contraseña debe contener al menos un carácter especial", nameof(value));
            else if (value.Length < 8)
                throw new ArgumentException("Contraseña debe tener al menos 8 caracteres");
            _contraseña = value;
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

    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña, Rol rol)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        Contraseña = contraseña;
        _roles.Add(rol);
    }
}