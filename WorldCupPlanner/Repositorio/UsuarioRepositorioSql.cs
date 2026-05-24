using Dominio.Clases;

namespace Repositorio;

public class UsuarioRepositorioSql
{
    private SqlContexto _contexto;

    public UsuarioRepositorioSql(SqlContexto contexto)
    {
        _contexto = contexto;
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _contexto.Usuarios.ToList();
    }
}