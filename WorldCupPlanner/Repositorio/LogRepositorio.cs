using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class LogRepositorio : ILogRepositorio
{
    private readonly BaseDeDatosEnMemoria _BDEnMemoria;
    public void Agregar(BaseDeDatosEnMemoria bdEnMemoria)
    {
        throw new NotImplementedException();
    }

    public List<Log> ObtenerTodos()
    {
        throw new NotImplementedException();
    }
}