using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class LogServiciosTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private ILogRepositorio _logRepositorio;
    private IServicioLog _logServicios;
    private LogDTO _logDTO;

    [TestInitialize]
    public void TestInitialize()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _logRepositorio = new LogRepositorio(_baseDeDatosEnMemoria);
        _logServicios = new LogServicios(_logRepositorio);

        _logDTO = new LogDTO()
        {
            mensaje = "Inicio de sistema",
            usuario = "admin@worldcup.com",
            tipo = TipoLog.Info,
            fechaISO8601 = DateTime.Now.ToString("o")
        };
    }

}