using Dominio.Clases;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class NotificacionRepositorioSqlTest
{
    private NotificacionRepositorioSql _notificacionRepositorioSql;
    private Notificacion _notificacion;
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void Inicializar()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _notificacionRepositorioSql = new NotificacionRepositorioSql(_contexto);
        DateTime fechaHora = DateTime.Now;
        Usuario usuario = new Usuario();
        _notificacion = new Notificacion("Mensaje", fechaHora, usuario.Id);
    }
}