using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface ILogRepositorio
{
    void Agregar(Log nuevoLog);
    List<Log> ObtenerTodos();
}