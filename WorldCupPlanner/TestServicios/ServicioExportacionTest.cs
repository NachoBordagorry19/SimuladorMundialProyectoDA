using Dominio.Clases;
using Servicios.Clases;

namespace TestServicios;

[TestClass]
public class ServicioExportacionTest
{
    private AuditoriaRepositorioMock _repoMock;
    private ServicioExportacion _servicio;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new AuditoriaRepositorioMock();
        _servicio = new ServicioExportacion(_repoMock);
    }

    [TestMethod]
    public void ExportarAuditoriaCsv_ConRegistros_DevuelveBytesNoVacios()
    {
        _repoMock.AgregarRegistro(new Auditoria { Usuario = "admin@test.com", Accion = "Alta Usuario", Detalle = "Test" });
        byte[] resultado = _servicio.ExportarAuditoriaCsv(DateTime.MinValue, DateTime.MaxValue);
        Assert.IsTrue(resultado.Length > 0);
    }

    [TestMethod]
    public void ExportarAuditoriaCsv_ConRegistros_ContieneEncabezado()
    {
        byte[] resultado = _servicio.ExportarAuditoriaCsv(DateTime.MinValue, DateTime.MaxValue);
        string contenido = System.Text.Encoding.UTF8.GetString(resultado);
        Assert.IsTrue(contenido.Contains("FechaHora,Usuario,Accion,Detalle"));
    }

    [TestMethod]
    public void ExportarAuditoriaXlsx_ConRegistros_DevuelveBytesNoVacios()
    {
        _repoMock.AgregarRegistro(new Auditoria { Usuario = "admin@test.com", Accion = "Alta Usuario", Detalle = "Test" });
        byte[] resultado = _servicio.ExportarAuditoriaXlsx(DateTime.MinValue, DateTime.MaxValue);
        Assert.IsTrue(resultado.Length > 0);
    }
}