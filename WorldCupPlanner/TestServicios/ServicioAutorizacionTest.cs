using Dominio.Enums;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class ServicioAutorizacionTest
{
    private IServicioAutorizacion _servicioAutorizacion;

    [TestInitialize]
    public void Inicializar()
    {
        _servicioAutorizacion = new ServicioAutorizacion();
    }

    [TestMethod]
    public void Administrador_PuedeGestionarUsuarios()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeGestionarUsuarios(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Administrador_PuedeGestionarEquipos()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeGestionarEquipos(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Administrador_PuedeGestionarEstadios()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeGestionarEstadios(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Administrador_PuedeVisualizarLogs()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeVisualizarLogs(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Editor_PuedeUsarFixture()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "editor@gmail.com",
            Roles = new List<Rol> { Rol.Editor }
        };

        bool resultado = _servicioAutorizacion.PuedeUsarFixture(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Editor_PuedeEditarPartidos()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "editor@gmail.com",
            Roles = new List<Rol> { Rol.Editor }
        };

        bool resultado = _servicioAutorizacion.PuedeEditarPartidos(usuario);

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Editor_PuedeImportar()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "editor@gmail.com",
            Roles = new List<Rol> { Rol.Editor }
        };

        bool resultado = _servicioAutorizacion.PuedeImportar(usuario);

        Assert.IsTrue(resultado);
    }
    [TestMethod]
    public void UsuarioConAmbosRoles_PuedeGestionarUsuariosYUsarFixture()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admineditor@gmail.com",
            Roles = new List<Rol> { Rol.Administrador, Rol.Editor }
        };

        bool puedeGestionarUsuarios = _servicioAutorizacion.PuedeGestionarUsuarios(usuario);
        bool puedeUsarFixture = _servicioAutorizacion.PuedeUsarFixture(usuario);

        Assert.IsTrue(puedeGestionarUsuarios);
        Assert.IsTrue(puedeUsarFixture);
    }
    
    [TestMethod]
    public void Administrador_PuedeEliminarOtroUsuario()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeEliminarUsuario(usuario, "otro@gmail.com");

        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void Administrador_NoPuedeEliminarSuPropiaCuenta()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeEliminarUsuario(usuario, "admin@gmail.com");

        Assert.IsFalse(resultado);
    }
    [TestMethod]
    public void Editor_NoPuedeGestionarUsuarios()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "editor@gmail.com",
            Roles = new List<Rol> { Rol.Editor }
        };

        bool resultado = _servicioAutorizacion.PuedeGestionarUsuarios(usuario);

        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void Administrador_NoPuedeImportar()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "admin@gmail.com",
            Roles = new List<Rol> { Rol.Administrador }
        };

        bool resultado = _servicioAutorizacion.PuedeImportar(usuario);

        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void Editor_NoPuedeEliminarUsuarios()
    {
        UsuarioDTO usuario = new UsuarioDTO
        {
            Email = "editor@gmail.com",
            Roles = new List<Rol> { Rol.Editor }
        };

        bool resultado = _servicioAutorizacion.PuedeEliminarUsuario(usuario, "otro@gmail.com");

        Assert.IsFalse(resultado);
    }
    
}