using Repositorio;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;
using Servicios.Clases;
using Dominio.Enums;
using Moq;

namespace TestServicios;

[TestClass]
public class UsuarioServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IUsuarioRepositorio _usuarioRepositorio;
    private ServicioUsuario _servicioUsuario;
    private UsuarioDTO _usuarioDTO;
    private Mock<IServicioAuditoria> _auditoriaMock;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _usuarioRepositorio = new UsuarioRepositorio(_baseDeDatosEnMemoria);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _servicioUsuario = new ServicioUsuario(_usuarioRepositorio, _auditoriaMock.Object);

        _usuarioDTO = new UsuarioDTO()
        {
            Nombre = "Fede",
            Apellido = "Rodriguez",
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15),
            Contraseña = "Password123!",
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
    public void AgregarUsuario_DebeRegistrarAuditoria()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        _auditoriaMock.Verify(a => a.RegistrarAltaUsuario("a@gmail.com", "Administrador"), Times.Once);
    }

    [TestMethod]
    public void ObtenerUsuarios_SeDevuelvenCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        UsuarioDTO usuario = new UsuarioDTO();
        usuario.Nombre = "Mateo";
        usuario.Apellido = "Roo";
        usuario.FechaNacimiento = new DateTime(2000, 05, 15);
        usuario.Contraseña = "Password123!";
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

    [TestMethod]
    public void AgregarUsuario_GuardaContraseñaCifrada()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        var usuarioGuardado = _usuarioRepositorio.ObtenerUsuario(u => u.Email == "a@gmail.com");

        Assert.IsNotNull(usuarioGuardado);
        Assert.AreNotEqual("Password123!", usuarioGuardado.Contraseña);
    }

    [TestMethod]
    public void AutenticarUsuario_SiDatosSonCorrectos_DevuelveUsuario()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuarioAutenticado = _servicioUsuario.AutenticarUsuario("a@gmail.com", "Password123!");

        Assert.AreEqual("Fede", usuarioAutenticado.Nombre);
        Assert.AreEqual("a@gmail.com", usuarioAutenticado.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AutenticarUsuario_SiContraseñaEsIncorrecta_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _servicioUsuario.AutenticarUsuario("a@gmail.com", "Incorrecta123!");
    }

    [TestMethod]
    public void ActualizarUsuario_ModificaDatos()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuarioActualizado = new UsuarioDTO()
        {
            Nombre = "Federico",
            Apellido = "Perez",
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(1999, 10, 10),
            Contraseña = "",
            Roles = new List<Rol> { Rol.Administrador, Rol.Editor }
        };

        _servicioUsuario.ActualizarUsuario(usuarioActualizado);

        UsuarioDTO usuarioObtenido = _servicioUsuario.ObtenerUsuario("a@gmail.com");

        Assert.AreEqual("Federico", usuarioObtenido.Nombre);
        Assert.AreEqual("Perez", usuarioObtenido.Apellido);
        Assert.IsTrue(usuarioObtenido.Roles.Contains(Rol.Administrador));
        Assert.IsTrue(usuarioObtenido.Roles.Contains(Rol.Editor));
    }
    
    [TestMethod]
    public void ActualizarUsuario_DebeRegistrarAuditoria()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        var usuarioEditado = new UsuarioDTO { 
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(1999, 10, 10),
            Nombre = "Fede Editado", 
            Apellido = "Rodriguez",
            Roles = new List<Rol> { Rol.Editor },
            Contraseña = ""
        };

        _servicioUsuario.ActualizarUsuario(usuarioEditado);
        _auditoriaMock.Verify(a => a.RegistrarEdicionUsuario("a@gmail.com"), Times.Once);
    }

    [TestMethod]
    public void ReiniciarContraseña_PermiteAutenticarConContraseñaPorDefecto()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _servicioUsuario.ReiniciarContraseña("a@gmail.com");

        UsuarioDTO usuarioAutenticado = _servicioUsuario.AutenticarUsuario("a@gmail.com", "Usuario123!");

        Assert.AreEqual("a@gmail.com", usuarioAutenticado.Email);
    }
    
    [TestMethod]
    public void ReiniciarContraseña_DebeRegistrarAuditoria()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);
        _servicioUsuario.ReiniciarContraseña("a@gmail.com");
        _auditoriaMock.Verify(a => a.RegistrarEdicionUsuario("a@gmail.com"), Times.AtLeast(1));
    }
}