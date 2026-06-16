using System;
using System.Linq;
using System.Collections.Generic;
using Dominio.Enums;

namespace Dominio.Clases;


public class Usuario
{
    private int _id;
    
    public int Id
    {
        get => _id;
        set => _id = value;
    }
    private string _nombre = string.Empty;
    private string _apellido = string.Empty;
    private string _email = string.Empty;
    private DateTime _fechaNacimiento;
    private string _contrasena = string.Empty;
    public IReadOnlyList<Rol> Roles => _roles.AsReadOnly();
    private readonly List<Rol> _roles = new();

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre es obligatorio");
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
                throw new ArgumentException("El apellido es obligatorio");
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
                throw new ArgumentException("El email es obligatorio");
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
                throw new ArgumentException("La fecha de nacimiento no es válida");
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

    public string Contrasena
    {
        get => _contrasena;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("La contrasena es obligatoria");
            }
            else if (!value.Any(char.IsUpper))
            {
                throw new ArgumentException("La contrasena debe contener al menos una letra mayúscula");
            }
            else if (!value.Any(char.IsLower))
            {
                throw new ArgumentException("La contrasena debe contener al menos una letra minúscula");
            }
            else if (!value.Any(char.IsDigit))
                throw new ArgumentException("La contrasena debe contener al menos un número");
            else if (!value.Any(c => !char.IsLetterOrDigit(c)))
                throw new ArgumentException("La contrasena debe contener al menos un carácter especial", nameof(value));
            else if (value.Length < 8)
                throw new ArgumentException("La contrasena debe tener al menos 8 caracteres");
            _contrasena = value;
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

    public Usuario()
    {

    }

    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contrasena, Rol rol)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        Contrasena = contrasena;
        _roles.Add(rol);
    }

    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contrasena, List<Rol> roles)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        FechaNacimiento = fechaNacimiento;
        Contrasena = contrasena;

        foreach (Rol rol in roles)
        {
            _roles.Add(rol);
        }
    }

}