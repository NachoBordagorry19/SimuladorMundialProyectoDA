using Dominio.Clases;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Repositorio;

namespace TestRepositorio;

[TestClass]
public class UsuarioRepositorioSqlTest
{
    private UsuarioRepositorioSql _usuarioRepositorioSql;
    private Usuario _usuario;
    private SqlContexto  _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Usuarios.RemoveRange(_contexto.Usuarios);
        _contexto.Auditorias.RemoveRange(_contexto.Auditorias);
        _contexto.Partidos.RemoveRange(_contexto.Partidos);
        _contexto.Grupos.RemoveRange(_contexto.Grupos);
        _contexto.Estadios.RemoveRange(_contexto.Estadios);
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _usuarioRepositorioSql = new UsuarioRepositorioSql(_contexto);
        _usuario = new Usuario("Fede", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "aveAA@e123", Rol.Administrador)
    }

    [TestMethod]
    public void ObtenerUsuarios()
    {
        List<Usuario> usuarios = _usuarioRepositorioSql.ObtenerUsuarios();
        Assert.AreEqual(0, usuarios.Count);
    }
}