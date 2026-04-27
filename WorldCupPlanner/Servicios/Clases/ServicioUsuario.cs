using System;
using System.Linq;
using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioUsuario:IServicioUsuario
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public ServicioUsuario(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public void AgregarUsuario(UsuarioDTO usuarioDto)
    {
        Usuario usuario = UsuarioDTOAEntidad(usuarioDto);
        string emailAVerificar = usuario.Email;
        ValidadEmail(emailAVerificar);
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

    public void ValidadEmail(string email)
    {
        Usuario usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
        if (usuarioExistente != null)
        {
            throw new ArgumentException("El usuario ya existe, porfavor agrege un usuario que no exista");
        }
    }

    private Usuario UsuarioDTOAEntidad(UsuarioDTO usuarioDto)
    {
        var usuario = new Usuario()
        {
            Nombre = usuarioDto.Nombre,
            Apellido = usuarioDto.Apellido,
            Email = usuarioDto.Email,
            FechaNacimiento = usuarioDto.FechaNacimiento,
        };
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
    
    
    
    
}