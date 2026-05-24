using Dominio.Clases;
using Microsoft.EntityFrameworkCore;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class UsuarioRepositorioSqlTest
{
    private UsuarioRepositorioSql _usuarioRepositorioSql;
    private Usuario _usuario;
    private SqlContexto  _contexto;

    [TestInitialize]
    public void IniciarPrueba()
    {
        
    }
}