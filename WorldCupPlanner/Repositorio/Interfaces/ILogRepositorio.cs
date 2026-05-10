using Dominio.Clases;
using Dominio.Enums;

namespace Repositorio.Interfaces;

public interface ILogRepositorio
{
    List<Log> ObtenerLogs();
    List<Log> ObtenerLogsPorTipo(TipoLog tipo);
    
    void AgregarLog(Log log);
}