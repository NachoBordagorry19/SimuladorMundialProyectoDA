namespace Servicios.Interfaces;

public interface IServicioAuditoria
{
    void RegistrarAltaUsuario(string email, string roles);
    void RegistrarEdicionUsuario(string email);
    List<string> ObtenerRegistrosFormateados();
}