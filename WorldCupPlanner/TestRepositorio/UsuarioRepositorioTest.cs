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
}