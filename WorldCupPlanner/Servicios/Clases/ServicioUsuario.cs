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
    private readonly IServicioAuditoria _auditoria;

    private const string ContraseñaPorDefecto = "Usuario123!";

    public ServicioUsuario(IUsuarioRepositorio usuarioRepositorio, IServicioAuditoria auditoria)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _auditoria = auditoria;
    }

    public void AgregarUsuario(UsuarioDTO usuarioDto)
    {
        ValidarContraseña(usuarioDto.Contraseña);
        Usuario usuario = UsuarioDTOAEntidad(usuarioDto);
        string emailAVerificar = usuario.Email;
        ValidarEmailExiste(emailAVerificar);
        ValidarRoles(usuarioDto);
        _usuarioRepositorio.AgregarUsuario(usuario);
        
        string rolesString = string.Join(", ", usuarioDto.Roles.Select(r => r.ToString()));
        _auditoria.RegistrarAltaUsuario(usuarioDto.Email, rolesString);
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
        Usuario? usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
        if (usuarioExistente != null)
        {
            throw new ArgumentException("El usuario ya existe, porfavor agrege un usuario que no exista");
        }
    }

    private void ValidarContraseña(string contraseña)
    {
        var usuarioTemporal = new Usuario("temp", "temp", "temp@temp.com", DateTime.Today.AddYears(-18), contraseña, Rol.Editor);
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
        Usuario usuario = ObtenerEntidadPorEmail(email);
        return desdeEntidad(usuario);
    }

    public void ValidarEmailNoExiste(string email)
    {
        Usuario? usuarioExistente = _usuarioRepositorio.ObtenerUsuario(u => u.Email == email);
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
    
    public List<UsuarioDTO> ObtenerUsuariosEliminables(string emailUsuarioActual)
    {
        return ObtenerUsuarios()
            .Where(usuario => !string.Equals(usuario.Email, emailUsuarioActual, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void EliminarUsuario(string emailSeleccionado, string emailConfirmado, string emailUsuarioActual)
    {
        if (string.IsNullOrWhiteSpace(emailSeleccionado))
        {
            throw new ArgumentException("Debe seleccionar un usuario");
        }

        if (string.IsNullOrWhiteSpace(emailConfirmado))
        {
            throw new ArgumentException("Debe confirmar el email");
        }

        if (!string.Equals(emailSeleccionado.Trim(), emailConfirmado.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("El email confirmado no coincide con el usuario seleccionado");
        }

        if (string.Equals(emailSeleccionado.Trim(), emailUsuarioActual.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("No puedes eliminar tu propia cuenta");
        }

        EliminarUsuario(new UsuarioDTO { Email = emailSeleccionado.Trim() });
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
        string emailNormalizado = email.Trim();

        Usuario usuario = ObtenerEntidadPorEmail(emailNormalizado);

        if (usuario.Contraseña != CifrarContraseña(contraseña))
        {
            throw new ArgumentException("Email o contraseña incorrectos");
        }

        return desdeEntidad(usuario);
    }

    public void ActualizarUsuario(UsuarioDTO usuarioDto)
    {
        ActualizarUsuario(usuarioDto.Email, usuarioDto);
    }

    public void ActualizarUsuario(string emailOriginal, UsuarioDTO usuarioDto)
    {
        Usuario usuarioExistente = ObtenerEntidadPorEmail(emailOriginal);

        if (!string.Equals(emailOriginal, usuarioDto.Email, StringComparison.OrdinalIgnoreCase))
        {
            ValidarEmailExiste(usuarioDto.Email);
        }

        ValidarRoles(usuarioDto);

        string contraseña = usuarioExistente.Contraseña;

        if (!string.IsNullOrWhiteSpace(usuarioDto.Contraseña))
        {
            ValidarContraseña(usuarioDto.Contraseña);
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

        _usuarioRepositorio.EliminarUsuario(usuarioExistente);
        _usuarioRepositorio.AgregarUsuario(usuarioActualizado);

        _auditoria.RegistrarEdicionUsuario(usuarioDto.Email);
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
        _auditoria.RegistrarEdicionUsuario(email);
    }
}