using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioUsuario : IServicioUsuario
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    
    private const string ContraseñaPorDefecto = "Usuario123!";
    
    public ServicioUsuario(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public void AgregarUsuario(UsuarioDTO usuarioDto)
    {
        Usuario usuario = UsuarioDTOAEntidad(usuarioDto);
        string emailAVerificar = usuario.Email;
        ValidarEmailExiste(emailAVerificar);
        ValidarRoles(usuarioDto);
        _usuarioRepositorio.AgregarUsuario(usuario);
    }

    public void ValidarRoles(UsuarioDTO usuarioDto)
    {
        var roles = usuarioDto.Roles;
        var duplicados = roles.GroupBy(r => r).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicados.Any())
        {
            throw new ArgumentException("El usuario no puede tener roles duplicados");
        }
    }

    public void ValidarEmailExiste(string email)
    {
        Usuario usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
        if (usuarioExistente != null)
        {
            throw new ArgumentException("El usuario ya existe, porfavor agrege un usuario que no exista");
        }
    }

    private Usuario UsuarioDTOAEntidad(UsuarioDTO usuarioDto)
    {
        var usuario = new Usuario(
            usuarioDto.Nombre,
            usuarioDto.Apellido,
            usuarioDto.Email,
            usuarioDto.FechaNacimiento,
            CifrarContraseña(usuarioDto.Contraseña),
            usuarioDto.Roles
        );

        return usuario;
    }

    private static UsuarioDTO desdeEntidad(Usuario usuario)
    {
        return new UsuarioDTO()
        {
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            FechaNacimiento = usuario.FechaNacimiento,
            Roles = usuario.Roles.ToList()
        };
    }

    public List<UsuarioDTO> ObtenerUsuarios()
    {
        List<UsuarioDTO> usuarioDTO = new List<UsuarioDTO>();
        foreach (var usuario in _usuarioRepositorio.ObtenerUsuarios())
        {
            usuarioDTO.Add(desdeEntidad(usuario));
        }
        return usuarioDTO;
    }

    public UsuarioDTO ObtenerUsuario(string email)
    {
        ValidarEmailNoExiste(email);
        Usuario? usuario = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
        return desdeEntidad(usuario);
    }

    public void ValidarEmailNoExiste(string email)
    {
        Usuario usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
        if (usuarioExistente == null)
        {
            throw new ArgumentException("El usuario no existe, porfavor ingrese un usuario que exista");
        }
    }

    public void EliminarUsuario(UsuarioDTO usuarioDto)
    {
        ValidarEmailNoExiste(usuarioDto.Email);
        var usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == usuarioDto.Email);
        if (usuarioExistente != null)
        {
            _usuarioRepositorio.EliminarUsuario(usuarioExistente);
        }
    }

    private string CifrarContraseña(string contraseña)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(contraseña);
        byte[] hash = SHA256.HashData(bytes);

        return "Aa1!" + Convert.ToHexString(hash);
    }

    private Usuario ObtenerEntidadPorEmail(string email)
    {
        Usuario? usuario = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);

        if (usuario == null)
        {
            throw new ArgumentException("El usuario no existe, porfavor ingrese un usuario que exista");
        }

        return usuario;
    }
    
    public UsuarioDTO AutenticarUsuario(string email, string contraseña)
    {
        Usuario usuario = ObtenerEntidadPorEmail(email);

        if (usuario.Contraseña != CifrarContraseña(contraseña))
        {
            throw new ArgumentException("Email o contraseña incorrectos");
        }

        return desdeEntidad(usuario);
    }

    public void ActualizarUsuario(UsuarioDTO usuarioDto)
    {
        Usuario usuarioExistente = ObtenerEntidadPorEmail(usuarioDto.Email);

        string contraseña = usuarioExistente.Contraseña;

        if (!string.IsNullOrWhiteSpace(usuarioDto.Contraseña))
        {
            contraseña = CifrarContraseña(usuarioDto.Contraseña);
        }

        Usuario usuarioActualizado = new Usuario(
            usuarioDto.Nombre,
            usuarioDto.Apellido,
            usuarioDto.Email,
            usuarioDto.FechaNacimiento,
            contraseña,
            usuarioDto.Roles
        );

        _usuarioRepositorio.ActualizarUsuario(usuarioActualizado);
    }

    public void ReiniciarContraseña(string email)
    {
        Usuario usuarioExistente = ObtenerEntidadPorEmail(email);

        Usuario usuarioActualizado = new Usuario(
            usuarioExistente.Nombre,
            usuarioExistente.Apellido,
            usuarioExistente.Email,
            usuarioExistente.FechaNacimiento,
            CifrarContraseña(ContraseñaPorDefecto),
            usuarioExistente.Roles.ToList()
        );

        _usuarioRepositorio.ActualizarUsuario(usuarioActualizado);
    }



}