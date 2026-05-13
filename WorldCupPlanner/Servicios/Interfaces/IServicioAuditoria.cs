namespace Servicios.Interfaces;

public interface IServicioAuditoria
{
    void RegistrarAltaUsuario(string email, string roles);
    List<string> ObtenerRegistrosFormateados();
    void RegistrarEdicionEquipo(string nombre);
    void RegistrarEliminacionEquipo(string nombre);
    void RegistrarEdicionUsuario(string email);
    void RegistrarEliminacionUsuario(string email);
    void RegistrarAltaEstadio(string nombre);
    void RegistrarEdicionEstadio(string nombre);
    void RegistrarEliminacionEstadio(string nombre);
    void RegistrarModificacionPartido(string detalle);
    void RegistrarImportacionEquipos(string mensaje, bool esExito);
    void RegistrarGeneracionAutomaticaEquipos(int cantidad);
    void RegistrarGeneracionFixture();
    void RegistrarSorteoCruces();
}