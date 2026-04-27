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
        _usuarioRepositorio.AgregarUsuario(usuario);
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
    
}