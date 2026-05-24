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

    public void AgregarUsuario(Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        _contexto.SaveChanges();
    }

    public void EliminarUsuario(Usuario usuario)
    {
        _contexto.Usuarios.Remove(usuario);
        _contexto.SaveChanges();
    }

    public Usuario? ObtenerUsuario(Func<Usuario, bool> filtro)
    {
        return _contexto.Usuarios.ToList().Where(filtro).FirstOrDefault();
    }
}