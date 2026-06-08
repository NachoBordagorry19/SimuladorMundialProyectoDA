using Moq;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class ServicioNotificacionTest
{
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;
    private INotificacionRepositorio _notificacionRepositorio;
    private ServicioNotificacion _servicioNotificacion;
    private NotificacionDTO _notDTO1;
    private NotificacionDTO _notDTO2;
    private Mock<IServicioAuditoria> _auditoriaMock;

    [TestInitialize]
    public void Inicializar()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _notificacionRepositorio = new NotificacionRepositorioSql(_contexto);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _servicioNotificacion = new ServicioNotificacion(_notificacionRepositorio, _auditoriaMock.Object);

        _notDTO1 = new NotificacionDTO()
        {
            Mensaje = "Notificacion 1",
            FechaHora = DateTime.Now,
            UsuarioId = 1,
            Leida = false
        };
        
        _notDTO2 = new NotificacionDTO()
        {
            Mensaje = "Notificacion 2",
            FechaHora = DateTime.Now,
            UsuarioId = 2,
            Leida = false
        };
    }
}