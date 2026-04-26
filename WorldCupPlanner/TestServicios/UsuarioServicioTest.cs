using Repositorio;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;
using Servicios.Clases;

namespace TestServicios;

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
        _servicioUsuario = new ServicioUsuario(_interfazServicioUsuario);

        _usuarioDTO = new UsuarioDTO()
        {
            Nombre = "Fede",
            Apellido = "Rodriguez",
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15)
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
}