using Repositorio;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;
using Servicios.Clases;
using Dominio.Enums;

namespace TestServicios;

[TestClass]
public class UsuarioServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IUsuarioRepositorio _usuarioRepositorio;
    private ServicioUsuario _servicioUsuario;
    private UsuarioDTO _usuarioDTO;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _usuarioRepositorio = new UsuarioRepositorio(_baseDeDatosEnMemoria);
        _servicioUsuario = new ServicioUsuario(_usuarioRepositorio);

        _usuarioDTO = new UsuarioDTO()
        {
            Nombre = "Fede",
            Apellido = "Rodriguez",
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15),
            Roles = new List<Rol> { Rol.Administrador }
        };
    }

    [TestMethod]
    public void AgregoUsuario_SiEmailNoExiste()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        Assert.AreEqual("Fede", _usuarioDTO.Nombre);
        Assert.AreEqual("Rodriguez", _usuarioDTO.Apellido);
        Assert.AreEqual("a@gmail.com", _usuarioDTO.Email);
        Assert.AreEqual(new DateTime(2000, 05, 15), _usuarioDTO.FechaNacimiento);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AregarUsuario_SiEmailExiste_LanzoExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiRolesDuplicados_LanzoExcepcion()
    {
        _usuarioDTO.Roles = new List<Rol> { Rol.Administrador, Rol.Administrador };
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }

    [TestMethod]
    public void ObtenerUsuarios_SeDevuelvenCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        UsuarioDTO usuario = new UsuarioDTO();
        usuario.Nombre = "Mateo";
        usuario.Apellido = "Roo";
        usuario.FechaNacimiento = new DateTime(2000, 05, 15);
        usuario.Email = "b@gmail.com";
        usuario.Roles = new List<Rol> { Rol.Editor };
        _servicioUsuario.AgregarUsuario(usuario);
        var usuarios = _servicioUsuario.ObtenerUsuarios();
        Assert.AreEqual(usuarios.Count(), 2);
    }

    [TestMethod]
    public void ObtenerUsuario_SeDevuelveUsuarioCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        UsuarioDTO usuarioDtoPrueba = _servicioUsuario.ObtenerUsuario("a@gmail.com");
        Assert.AreEqual(usuarioDtoPrueba.Nombre, "Fede");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerUsuario_SiNoExiste_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        UsuarioDTO usuarioDtoPrueba = _servicioUsuario.ObtenerUsuario("Roo");
    }

    [TestMethod]
    public void EliminarUsuario_SeEliminaCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        _servicioUsuario.EliminarUsuario(_usuarioDTO);
        var listaUsuarios = _servicioUsuario.ObtenerUsuarios();
        Assert.AreEqual(0, listaUsuarios.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuario_SiNoExiste_LanzoExcepcion()
    {
        _servicioUsuario.EliminarUsuario(_usuarioDTO);
    }
}