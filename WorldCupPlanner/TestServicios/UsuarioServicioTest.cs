using Repositorio;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

public class UsuarioServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IServicioUsuario _interfazServicioUsuario;
    private ServicioUsuario _servicioUsuario;
    private UsuarioDTO _usuarioDTO;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _interfazServicioUsuario = new UsuarioRepositorio(_baseDeDatosEnMemoria);
        _servicioUsuario = new ServicioUsuario(_interfazServicioUsuario);

        _usuarioDTO = new UsuarioDTO()
        {
            Nombre = "Fede",
            Apellido = "Rodriguez",
            Email = "a@gmail.com",
            FechaNacimiento = new DateTime(2000, 05, 15)
        };
    }
}