using System.Globalization;
using Dominio.Clases;
using Dominio.Enums;
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
    private IUsuarioRepositorio _usuarioRepositorio;
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
        _usuarioRepositorio = new UsuarioRepositorioSql(_contexto);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _servicioNotificacion = new ServicioNotificacion(_notificacionRepositorio, _usuarioRepositorio, _auditoriaMock.Object);

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
    
    [TestMethod]
    public void GenerarNotificaciones_DeberiaGenerarNotificacionesParaTodosLosPeriodistas()
    {
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario1 = new Usuario ("Usuario1", "Apellido", "usuario1@gmail.com", fechaHora, "Contraseña_1", Rol.Periodista);
        Usuario usuario2 = new Usuario ("Usuario2", "Apellido", "usuario2@gmail.com", fechaHora, "Contraseña_2", Rol.Periodista);
        Usuario usuario3 = new Usuario ("Usuario3", "Apellido", "usuario3@gmail.com", fechaHora, "Contraseña_3", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario1);
        _usuarioRepositorio.AgregarUsuario(usuario2);
        _usuarioRepositorio.AgregarUsuario(usuario3);
        string mensaje = "Notificacion Generada";
        _servicioNotificacion.GenerarNotificaciones(mensaje);
        var notificacionesUsuario1 = _servicioNotificacion.ObtenerNoLeidas(1);
        var notificacionesUsuario2 = _servicioNotificacion.ObtenerNoLeidas(2);
        var notificacionesUsuario3 = _servicioNotificacion.ObtenerNoLeidas(3);
        
        Assert.IsTrue(notificacionesUsuario1.Any(not => not.Mensaje == mensaje));
        Assert.AreEqual(1, notificacionesUsuario1.Count);
        Assert.IsTrue(notificacionesUsuario2.Any(not => not.Mensaje == mensaje));
        Assert.AreEqual(1, notificacionesUsuario2.Count);
        Assert.IsTrue(notificacionesUsuario3.Any(not => not.Mensaje == mensaje));
        Assert.AreEqual(1, notificacionesUsuario3.Count);
    }

    [TestMethod]
    public void GenerarNotificaciones_SoloGeneraParaPeriodistas()
    {
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario1 = new Usuario ("Usuario1", "Apellido", "usuario1@gmail.com", fechaHora, "Contraseña_1", Rol.Administrador);
        Usuario usuario2 = new Usuario ("Usuario2", "Apellido", "usuario2@gmail.com", fechaHora, "Contraseña_2", Rol.Editor);
        Usuario usuario3 = new Usuario ("Usuario3", "Apellido", "usuario3@gmail.com", fechaHora, "Contraseña_3", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario1);
        _usuarioRepositorio.AgregarUsuario(usuario2);
        _usuarioRepositorio.AgregarUsuario(usuario3);
        string mensaje = "Notificacion Generada";
        _servicioNotificacion.GenerarNotificaciones(mensaje);
        var notificacionesUsuario1 = _servicioNotificacion.ObtenerNoLeidas(1);
        var notificacionesUsuario2 = _servicioNotificacion.ObtenerNoLeidas(2);
        var notificacionesUsuario3 = _servicioNotificacion.ObtenerNoLeidas(3);
        
        Assert.AreEqual(0, notificacionesUsuario1.Count);
        Assert.AreEqual(0, notificacionesUsuario2.Count);
        Assert.IsTrue(notificacionesUsuario3.Any(not => not.Mensaje == mensaje));
        Assert.AreEqual(1, notificacionesUsuario3.Count);
    }

    [TestMethod]
    public void MarcarComoLeida_MarcaCorrectamente()
    {
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario = new Usuario ("Usuario1", "Apellido", "usuario1@gmail.com", fechaHora, "Contraseña_1", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario);
        _servicioNotificacion.GenerarNotificaciones("mensaje");
        var notificacionesUsuario = _servicioNotificacion.ObtenerNoLeidas(1);
        var not = notificacionesUsuario[0];
        _servicioNotificacion.MarcarComoLeida(not);
        Assert.IsTrue(not.Leida);
    }
    
    [TestMethod]
    public void GenerarNotificaciones_DebeLlamarAuditoria()
    {
        string mensaje = "Mensaje";
        string nombreUsuario = "Usuario";
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario = new Usuario("Usuario", "Apellido", "usuario@gmail.com", fechaHora, "Contraseña_1", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario);
        _servicioNotificacion.GenerarNotificaciones(mensaje);
        _auditoriaMock.Verify(a => a.RegistrarGeneracionNotificacion(nombreUsuario, mensaje));
    }

    [TestMethod]
    public void GenerarNotificaciones_ConEtiquetaGrupo_AsignaEtiquetaCorrectamente()
    {
        string mensaje = "Mensaje";
        string nombreUsuario = "Usuario";
        string etiqueta = "Etiqueta";
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario = new Usuario("Usuario", "Apellido", "usuario@gmail.com", fechaHora, "Contraseña_1", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario);
        _servicioNotificacion.GenerarNotificaciones(mensaje, etiqueta);
        Assert.AreEqual(etiqueta, _contexto.Notificaciones.First().EtiquetaGrupo);
    }

    [TestMethod]
    public void MarcarComoLeida_LlamaAuditoria()
    {
        string mensaje = "mensaje";
        DateTime fechaHora = DateTime.Parse("2003-12-14 14:30:00");
        Usuario usuario = new Usuario("Usuario", "Apellido", "usuario@gmail.com", fechaHora, "Contraseña_1", Rol.Periodista);
        _usuarioRepositorio.AgregarUsuario(usuario);
        _servicioNotificacion.GenerarNotificaciones(mensaje);
        var not = _servicioNotificacion.ObtenerNoLeidas(1)[0];
        _servicioNotificacion.MarcarComoLeida(not);
        _auditoriaMock.Verify(a => a.RegistrarLecturaNotificacion(mensaje));
    }
}
