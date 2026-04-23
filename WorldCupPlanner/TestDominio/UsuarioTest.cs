namespace TestDominio;

[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuario()
    {
        Usuario usuarioPrueba = new Usuario("Fede", "Gonzales", "a@gmail.com", DateTime(2003, 08, 23), "ave123");
    }
}