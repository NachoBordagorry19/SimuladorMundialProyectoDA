namespace Servicios.Interfaces;

public interface IServicioAuditoria
{
    void RegistrarAltaUsuario(string email, string roles);
    void RegistrarEdicionUsuario(string email);
    void RegistrarEliminacionUsuario(string email);
    void RegistrarAltaEstadio(string nombre);
    void RegistrarEdicionEstadio(string nombre);
    void RegistrarEliminacionEstadio(string nombre);
    void RegistrarAltaEquipo(string nombre);
    void RegistrarEdicionEquipo(string nombre);
    void RegistrarEliminacionEquipo(string nombre);
    void RegistrarGeneracionAutomaticaEquipos(int cantidad);
    void RegistrarGeneracionFixture();
    void RegistrarModificacionPartido(string detalle);
    void RegistrarSorteoCruces();
    void RegistrarImportacionEquipos(string mensaje, bool esExito);
    void RegistrarSimulacion(int semilla);
    void RegistrarCalculosDeCambioDeRanking(int valorAnterior, int valorNuevo);
    List<string> ObtenerRegistrosFormateados();
}