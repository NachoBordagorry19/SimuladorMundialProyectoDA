using Dominio.Clases;
using Dominio.Enums;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class UsuarioRepositorioTest
{
    private BaseDeDatosEnMemoria _BDenMemoria = null;
    private UsuarioRepositorio _repositorioUsuario = null;

    [TestInitialize]
    public void Inicializar()
    {
        _BDenMemoria = new BaseDeDatosEnMemoria();
        _repositorioUsuario = new UsuarioRepositorio(_BDenMemoria);
    }

    private static Usuario CrearUsuario(
        string nombre = "Juan",
        string apellido = "Gonzales",
        string email = "a@gmail.com",
        string contraseña = "aveAA@e123",
        Rol rol = Rol.Editor)
    {
        var nacimiento = DateTime.UtcNow.AddYears(-20);
        return new Usuario(nombre, apellido, email, nacimiento, contraseña, rol);
    }

    [TestMethod]
    public void ObtenerUsuarios_CuandoNoHayUsuarios_RetornaListaVacia()
    {
        var usuariosPrueba = _repositorioUsuario.ObtenerUsuarios();
        Assert.IsNotNull(usuariosPrueba);
        Assert.AreEqual(0, usuariosPrueba.Count);
    }

    [TestMethod]
    public void ObtenerUsuarios_HayUsuarioAgregado()
    {
        var usuario = CrearUsuario();
        _repositorioUsuario.AgregarUsuario(usuario);
        var usuariosLista = _repositorioUsuario.ObtenerUsuarios();
        Assert.IsNotNull(usuariosLista);
        Assert.AreEqual(1, usuariosLista.Count);
    }

    [TestMethod]
    public void ObtenerUsuarios_SiBuscoPorEmail_DevuelveCoincidencia()
    {
        var usuario = CrearUsuario();
        var usuario2 = CrearUsuario("Luis", "Pedro", "Pedro@gmail.com");
        _repositorioUsuario.AgregarUsuario(usuario);
        _repositorioUsuario.AgregarUsuario(usuario2);
        var encontrado = _repositorioUsuario.ObtenerUsuario(u => u.Email == "Pedro@gmail.com");
        Assert.IsNotNull(encontrado);
        Assert.AreSame(usuario2, encontrado);
    }

    [TestMethod]
    public void ModificarUsuarioExitente_BuscadoPorEmial()
    {
        var usuario = CrearUsuario("Alfredo", "Martinez", "Alfredo@gmail.com");
        _repositorioUsuario.AgregarUsuario(usuario);
        usuario.Apellido = "Gonzales";
        usuario.Nombre = "Fede";
        usuario.Email = "a@gmail.com";
        _repositorioUsuario.ActualizarUsuario(usuario);
        var actualizado = _repositorioUsuario.ObtenerUsuario(u => u.Email == "a@gmail.com");
        Assert.IsNotNull(actualizado);
        Assert.AreEqual("Fede", actualizado.Nombre);
        Assert.AreEqual("Gonzales", actualizado.Apellido);
    }

    [TestMethod]
    public void BorrarUsuario_SiUsuarioExiste_DejoListaVacia()
    {
        var usuario = CrearUsuario();
        _repositorioUsuario.AgregarUsuario(usuario);
        Assert.AreEqual(1, _repositorioUsuario.ObtenerUsuarios().Count);
        _repositorioUsuario.EliminarUsuario(usuario);
        Assert.AreEqual(0, _repositorioUsuario.ObtenerUsuarios().Count);
        var usuarioBorrado = _repositorioUsuario.ObtenerUsuario(u => u.Email == "a@gmail.com");
        Assert.IsNull(usuarioBorrado);
    }
}