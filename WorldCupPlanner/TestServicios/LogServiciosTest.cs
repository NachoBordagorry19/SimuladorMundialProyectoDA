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
    public void GuardarLog_CuandoFechaTieneFormatoInvalido_AsignaFechaActual()
    {
        LogDTO log = new LogDTO()
        {
            mensaje = "Mensaje",
            usuario = "admin@worldcup.com",
            tipo = TipoLog.Info,
            fechaISO8601 = "Fecha Invalida"
        };

        _logServicios.GuardarLog(log);
        var logs = _logServicios.ObtenerLogs();
        StringAssert.Contains(logs[0].fechaISO8601, DateTime.Now.ToString("yyyy-MM-dd"));
    }
    
    [TestMethod]
    public void GuardarLog_VerificarQueTodosLosCamposSeGuardanCorrectamente()
    {
        var fechaEspecifica = new DateTime(2026, 05, 10, 15, 30, 0);
        var logA = new LogDTO()
        {
            mensaje = "Mensaje",
            usuario = "analista@worldcup.com",
            tipo = TipoLog.Advertencia,
            fechaISO8601 = fechaEspecifica.ToString("o")
        };

        _logServicios.GuardarLog(logA);
        var listaDeLogs = _logServicios.ObtenerLogs();
        var logB = listaDeLogs[0];

        Assert.AreEqual(logA.mensaje, logB.mensaje);
        Assert.AreEqual(logA.usuario, logB.usuario);
        Assert.AreEqual(logA.tipo, logB.tipo);
        StringAssert.Contains(logB.fechaISO8601, "2026-05-10");
    }
}