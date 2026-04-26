using Dominio.Clases;
using Dominio.Enums;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class UsuarioRepositorioTest
{
    private BaseDeDatosEnMemoria BDenMemoria = null;

    [TestInitialize]
    public void Inicializar()
    {
        BDenMemoria = new BaseDeDatosEnMemoria();
        _repoUsuario = new RepositorioUsuario();
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
        var usuariosPrueba = _repoUsuarios.ObtenerUsuarios();
        Assert.IsNotNull(usuariosPrueba);
        Assert.AreEqual(0,usuariosPrueba.Count);
    }
}