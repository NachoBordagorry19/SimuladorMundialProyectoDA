using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;

namespace Repositorio;

public class LogRepositorio : ILogRepositorio
{
    private readonly BaseDeDatosEnMemoria _BDEnMemoria;
    public LogRepositorio(BaseDeDatosEnMemoria BDEnMemoria)
    {
        _BDEnMemoria = BDEnMemoria;
    }

    public List<Log> ObtenerLogs()
    {
        return _BDEnMemoria.ObtenerLogs();
    }
    
    public List<Log> ObtenerLogsPorTipo(TipoLog tipo)
    {
        List<Log> logs = _BDEnMemoria.ObtenerLogs();
        List<Log> logsPorTipo = new List<Log>();

        foreach (Log log in logs)
        {
            if (log.Tipo == tipo)
            {
                logsPorTipo.Add(log);
            }
        }
        return logsPorTipo;
    }

    public void AgregarLog(Log log)
    {
        _BDEnMemoria.AgregarLog(log);
    }
}