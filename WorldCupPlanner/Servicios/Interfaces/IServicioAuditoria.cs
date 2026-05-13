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
}