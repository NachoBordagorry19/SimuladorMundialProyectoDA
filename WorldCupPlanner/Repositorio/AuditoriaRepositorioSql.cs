using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class AuditoriaRepositorioSql : IAuditoriaRepositorio
{
    private SqlContexto _contexto;

    public AuditoriaRepositorioSql(SqlContexto contexto)
    {
        
    }
    public void AgregarRegistro(Auditoria registro)
    {
        throw new NotImplementedException();
    }

    public List<Auditoria> ObtenerTodosLosRegistros()
    {
        throw new NotImplementedException();
    }
}