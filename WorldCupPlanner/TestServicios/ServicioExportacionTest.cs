using Dominio.Clases;
using Dominio.Enums;
using Moq;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class ServicioExportacionTest
{
    private AuditoriaRepositorioMock _repoMock;
    private Mock<IServicioPartido> _servicioPartidoMock;
    private ServicioExportacion _servicio;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new AuditoriaRepositorioMock();
        _servicioPartidoMock = new Mock<IServicioPartido>();
        _servicio = new ServicioExportacion(_repoMock, _servicioPartidoMock.Object);
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

    [TestMethod]
    public void ExportarFixtureCsv_ConPartidosJugados_DevuelveBytesNoVacios()
    {
        _servicioPartidoMock.Setup(s => s.ObtenerPartidos()).Returns(new List<PartidoDTO>
        {
            new PartidoDTO
            {
                equipoLocal = new EquipoDTO { nombre = "Uruguay" },
                equipoVisitante = new EquipoDTO { nombre = "Brasil" },
                golesLocal = 2,
                golesVisitante = 1,
                estadoPartido = EstadoPartido.Jugado,
                fase = Fase.Grupos
            }
        });
        byte[] resultado = _servicio.ExportarFixtureCsv();
        Assert.IsTrue(resultado.Length > 0);
    }

    [TestMethod]
    public void ExportarFixtureXlsx_ConPartidosJugados_DevuelveBytesNoVacios()
    {
        _servicioPartidoMock.Setup(s => s.ObtenerPartidos()).Returns(new List<PartidoDTO>
        {
            new PartidoDTO
            {
                equipoLocal = new EquipoDTO { nombre = "Uruguay" },
                equipoVisitante = new EquipoDTO { nombre = "Brasil" },
                golesLocal = 2,
                golesVisitante = 1,
                estadoPartido = EstadoPartido.Jugado,
                fase = Fase.Grupos
            }
        });
        byte[] resultado = _servicio.ExportarFixtureXlsx();
        Assert.IsTrue(resultado.Length > 0);
    }
}