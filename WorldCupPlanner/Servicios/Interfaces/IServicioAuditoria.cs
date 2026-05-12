namespace Servicios.Interfaces;

public interface IServicioAuditoria
{
    void RegistrarAltaUsuario(string email, string roles);
    List<string> ObtenerRegistrosFormateados();
}