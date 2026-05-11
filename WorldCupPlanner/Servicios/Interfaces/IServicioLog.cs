using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioLog
{
    void GuardarLog(LogDTO dto);
    List<LogDTO> ObtenerLogs();
    List<LogDTO> ObtenerLogsPorTipo(TipoLog tipo);
    void RegistrarAltaEquipo(string nombre, string confederacion);
    void RegistrarEdicionEquipo(string nombre);
    void RegistrarEliminacionEquipo(string nombre);
}