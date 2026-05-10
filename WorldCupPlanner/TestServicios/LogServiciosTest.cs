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
    
    [TestMethod]
    public void GuardarLog_SePersisteCorrectamente()
    {
        _logServicios.GuardarLog(_logDTO);
    
        var logs = _logServicios.ObtenerLogs();
        Assert.IsNotNull(logs);
        Assert.AreEqual(1, logs.Count);
    }
    
    [TestMethod]
    public void GuardarLog_SiFechaEsVacia_AsignaFechaActual()
    {
        LogDTO log = new LogDTO()
        {
            mensaje = "Inicio de sistema",
            usuario = "admin@worldcup.com",
            tipo = TipoLog.Info,
            fechaISO8601 = ""
        };

        _logServicios.GuardarLog(log);
        var logs = _logServicios.ObtenerLogs();
        string fechaResultante = logs[0].fechaISO8601;

        Assert.IsNotNull(fechaResultante);
        string hoy = DateTime.Now.ToString("yyyy/MM/dd");
        StringAssert.Contains(fechaResultante, hoy);
        StringAssert.Contains(fechaResultante, "T");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GuardarLog_CuandoMensajeEsVacio_LanzaExcepcion()
    {
        LogDTO log = new LogDTO()
        {
            mensaje = "",
            usuario = "admin@worldcup.com",
            tipo = TipoLog.Info,
            fechaISO8601 = DateTime.Now.ToString("o")
        };

        _logServicios.GuardarLog(log);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GuardarLog_CuandoUsuarioEsNulo_LanzaExcepcion()
    {
        LogDTO log = new LogDTO()
        {
            mensaje = "Inicio de sistema",
            usuario = null,
            tipo = TipoLog.Info,
            fechaISO8601 = DateTime.Now.ToString("o")
        };

        _logServicios.GuardarLog(log);
    }
}