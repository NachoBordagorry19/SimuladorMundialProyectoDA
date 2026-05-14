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
    public void AutenticarUsuario_SiEmailTieneEspaciosAlPrincipioOFinal_DevuelveUsuario()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuarioAutenticado = _servicioUsuario.AutenticarUsuario("  a@gmail.com  ", "Password123!");

        Assert.AreEqual("a@gmail.com", usuarioAutenticado.Email);
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

        var usuarioEditado = new UsuarioDTO
        {
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
    
    [TestMethod]
    public void AgregarUsuario_SiEmailTieneEspacios_LoGuardaSinEspacios()
    {
        _usuarioDTO.Email = "  a@gmail.com  ";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuario = _servicioUsuario.ObtenerUsuario("a@gmail.com");

        Assert.AreEqual("a@gmail.com", usuario.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiEmailEstaVacio_LanzaExcepcion()
    {
        _usuarioDTO.Email = " ";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiEmailNoTieneArroba_LanzaExcepcion()
    {
        _usuarioDTO.Email = "agmail.com";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiEmailEmpiezaConArroba_LanzaExcepcion()
    {
        _usuarioDTO.Email = "@gmail.com";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiEmailTerminaConArroba_LanzaExcepcion()
    {
        _usuarioDTO.Email = "a@";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiEmailTieneDosArrobas_LanzaExcepcion()
    {
        _usuarioDTO.Email = "a@@gmail.com";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuario_SiContraseñaNoEsValida_LanzaExcepcion()
    {
        _usuarioDTO.Contraseña = "sinmayuscula1!";

        _servicioUsuario.AgregarUsuario(_usuarioDTO);
    }
    
    [TestMethod]
    public void ObtenerUsuario_SiEmailTieneEspacios_LoEncuentra()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuario = _servicioUsuario.ObtenerUsuario("  a@gmail.com  ");

        Assert.AreEqual("Fede", usuario.Nombre);
    }
    
    [TestMethod]
    public void AutenticarUsuario_SiEmailTieneEspacios_AutenticaCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuario = _servicioUsuario.AutenticarUsuario("  a@gmail.com  ", "Password123!");

        Assert.AreEqual("a@gmail.com", usuario.Email);
    }
    
    [TestMethod]
    public void ObtenerUsuariosEliminables_NoIncluyeUsuarioActual()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO otroUsuario = new UsuarioDTO
        {
            Nombre = "Mateo",
            Apellido = "Roo",
            Email = "b@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15),
            Contraseña = "Password123!",
            Roles = new List<Rol> { Rol.Editor }
        };

        _servicioUsuario.AgregarUsuario(otroUsuario);

        var eliminables = _servicioUsuario.ObtenerUsuariosEliminables("A@GMAIL.COM");

        Assert.AreEqual(1, eliminables.Count);
        Assert.AreEqual("b@gmail.com", eliminables[0].Email);
    }
    
    [TestMethod]
    public void EliminarUsuario_ConEmailsConfirmados_EliminaUsuario()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _servicioUsuario.EliminarUsuario("a@gmail.com", "A@GMAIL.COM", "admin@gmail.com");

        Assert.AreEqual(0, _servicioUsuario.ObtenerUsuarios().Count);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuario_SiNoSeleccionaUsuario_LanzaExcepcion()
    {
        _servicioUsuario.EliminarUsuario("", "a@gmail.com", "admin@gmail.com");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuario_SiNoConfirmaEmail_LanzaExcepcion()
    {
        _servicioUsuario.EliminarUsuario("a@gmail.com", "", "admin@gmail.com");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuario_SiConfirmacionNoCoincide_LanzaExcepcion()
    {
        _servicioUsuario.EliminarUsuario("a@gmail.com", "otro@gmail.com", "admin@gmail.com");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuario_SiIntentaEliminarseASiMismo_LanzaExcepcion()
    {
        _servicioUsuario.EliminarUsuario("a@gmail.com", "a@gmail.com", "A@GMAIL.COM");
    }
    
    [TestMethod]
    public void ActualizarUsuario_SiCambiaEmail_ActualizaCorrectamente()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO usuarioActualizado = new UsuarioDTO
        {
            Nombre = "Fede",
            Apellido = "Rodriguez",
            Email = "nuevo@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15),
            Contraseña = "",
            Roles = new List<Rol> { Rol.Administrador }
        };

        _servicioUsuario.ActualizarUsuario("a@gmail.com", usuarioActualizado);

        UsuarioDTO usuario = _servicioUsuario.ObtenerUsuario("nuevo@gmail.com");

        Assert.AreEqual("nuevo@gmail.com", usuario.Email);
    }
    
    [TestMethod]
    public void ActualizarUsuario_SiCambiaContraseña_PermiteAutenticarConNueva()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _usuarioDTO.Contraseña = "Nueva123!";

        _servicioUsuario.ActualizarUsuario("a@gmail.com", _usuarioDTO);

        UsuarioDTO usuario = _servicioUsuario.AutenticarUsuario("a@gmail.com", "Nueva123!");

        Assert.AreEqual("a@gmail.com", usuario.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarUsuario_SiEmailOriginalNoExiste_LanzaExcepcion()
    {
        _servicioUsuario.ActualizarUsuario("noexiste@gmail.com", _usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarUsuario_SiNuevoEmailYaExiste_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        UsuarioDTO otroUsuario = new UsuarioDTO
        {
            Nombre = "Mateo",
            Apellido = "Roo",
            Email = "b@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15),
            Contraseña = "Password123!",
            Roles = new List<Rol> { Rol.Editor }
        };

        _servicioUsuario.AgregarUsuario(otroUsuario);

        _usuarioDTO.Email = "b@gmail.com";

        _servicioUsuario.ActualizarUsuario("a@gmail.com", _usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarUsuario_SiRolesDuplicados_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _usuarioDTO.Roles = new List<Rol> { Rol.Editor, Rol.Editor };

        _servicioUsuario.ActualizarUsuario("a@gmail.com", _usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarUsuario_SiNuevoEmailNoTieneFormatoValido_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _usuarioDTO.Email = "emailinvalido";

        _servicioUsuario.ActualizarUsuario("a@gmail.com", _usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarUsuario_SiNuevaContraseñaNoEsValida_LanzaExcepcion()
    {
        _servicioUsuario.AgregarUsuario(_usuarioDTO);

        _usuarioDTO.Contraseña = "invalida";

        _servicioUsuario.ActualizarUsuario("a@gmail.com", _usuarioDTO);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ReiniciarContraseña_SiUsuarioNoExiste_LanzaExcepcion()
    {
        _servicioUsuario.ReiniciarContraseña("noexiste@gmail.com");
    }
}