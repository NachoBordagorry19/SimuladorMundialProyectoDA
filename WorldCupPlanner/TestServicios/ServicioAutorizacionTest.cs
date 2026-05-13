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
}