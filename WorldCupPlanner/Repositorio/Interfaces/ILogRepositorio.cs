using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface ILogRepositorio
{
    List<Log> ObtenerLogs();
    void AgregarLog(Log log);
}