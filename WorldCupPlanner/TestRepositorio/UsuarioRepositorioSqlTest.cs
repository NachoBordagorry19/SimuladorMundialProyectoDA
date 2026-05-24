using Dominio.Clases;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class UsuarioRepositorioSqlTest
{
    private UsuarioRepositorioSql _usuarioRepositorioSql;
    private Usuario _usuario;
    private SqlContexto  _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Usuarios.RemoveRange(_contexto.Usuarios);
        _contexto.SaveChanges();
        _usuarioRepositorioSql = new UsuarioRepositorioSql(_contexto);
        _usuario = new Usuario("Fede", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "aveAA@e123",
            Rol.Administrador);
    }

    [TestMethod]
    public void ObtenerUsuarios()
    {
        List<Usuario> usuarios = _usuarioRepositorioSql.ObtenerUsuarios();
        Assert.AreEqual(0, usuarios.Count);
    }

    [TestMethod]
    public void AgregarUsuario_SiUsuarioValidoSeAgrega()
    {
        _usuarioRepositorioSql.AgregarUsuario(_usuario);
        List<Usuario> usuarios = _usuarioRepositorioSql.ObtenerUsuarios();
        Assert.AreEqual(1, usuarios.Count);
    }

    [TestMethod]
    public void EliminarUsuario_SiUsuarioValidoSeElimina()
    {
        _usuarioRepositorioSql.AgregarUsuario(_usuario);
        List<Usuario> usuarios = _usuarioRepositorioSql.ObtenerUsuarios();
        Assert.AreEqual(1, usuarios.Count);
        _usuarioRepositorioSql.EliminarUsuario(_usuario);
        usuarios = _usuarioRepositorioSql.ObtenerUsuarios();
        Assert.AreEqual(0, usuarios.Count);
    }

    [TestMethod]
    public void ObtenerUsuario()
    {
        _usuarioRepositorioSql.AgregarUsuario(_usuario);
        Usuario usuarioPrueba = _usuarioRepositorioSql.ObtenerUsuario(u => u.Email == "a@gmail.com");
        Assert.AreEqual("a@gmail.com", usuarioPrueba.Email);
    }

    [TestMethod]
    public void ActualizarUsuario()
    {
        _usuarioRepositorioSql.AgregarUsuario(_usuario);
        _usuario.Apellido = "Ramirez";
        _usuarioRepositorioSql.ActualizarUsuario(_usuario);
        Usuario usuarioPrueba = _usuarioRepositorioSql.ObtenerUsuario(u => u.Email == "a@gmail.com");
        Assert.AreEqual("Ramirez",usuarioPrueba.Apellido);
    }
}