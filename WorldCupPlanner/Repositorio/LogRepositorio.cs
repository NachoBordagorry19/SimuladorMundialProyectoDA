using Dominio.Clases;
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

    public void AgregarLog(Log log)
    {
        _BDEnMemoria.AgregarLog(log);
    }
}