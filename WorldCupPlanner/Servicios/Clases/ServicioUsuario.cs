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
        _usuarioRepositorio.AgregarUsuario(usuario);
    }

    private Usuario UsuarioDTOAEntidad(UsuarioDTO usuarioDto)
    {
        return new Usuario()
        {
            Nombre = usuarioDto.Nombre,
            Apellido = usuarioDto.Apellido,
            Email = usuarioDto.Email,
            FechaNacimiento = usuarioDto.FechaNacimiento,
        };
    }
    
}