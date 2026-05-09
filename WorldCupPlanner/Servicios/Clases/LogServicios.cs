using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class LogServicios : IServicioLog
{
    private readonly ILogRepositorio _logRepo;

    public LogServicios(ILogRepositorio logRepo)
    {
        _logRepo = logRepo;
    }

    public void GuardarLog(LogDTO dto)
    {
    }

    public List<LogDTO> ObtenerLogs()
    {
        return new List<LogDTO>();
    }
}