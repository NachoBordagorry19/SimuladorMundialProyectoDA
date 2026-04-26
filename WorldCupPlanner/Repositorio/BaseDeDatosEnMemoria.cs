using Dominio.Clases;

namespace Repositorio;

public class BaseDeDatosEnMemoria
{
    private List<Usuario> _listaDeUsuarios { get; }
    private List<Equipo> _listaDeEquipos { get; }
    private List<Estadio> _listaDeEstadios { get; }
    private List<Partido> _listaDePartidos { get; }

    public BaseDeDatosEnMemoria()
    {
        _listaDeUsuarios = new List<Usuario>();
        _listaDeEquipos = new List<Equipo>();
        _listaDeEstadios = new List<Estadio>();
        _listaDePartidos = new List<Partido>();
    }

    public void AgregarUsuario(Usuario usuario)
    {
        _listaDeUsuarios.Add(usuario);
    }

    public void BorrarUsuario(Usuario usuario)
    {
        _listaDeUsuarios.Remove(usuario);
    }

    public void ActualizarUsuario(Usuario usuario)
    {
        Usuario? usuarioParaActualizar = _listaDeUsuarios.Find(u => u.Email == usuario.Email);
        var indiceUsuario = _listaDeUsuarios.IndexOf(usuarioParaActualizar);
        _listaDeUsuarios[indiceUsuario] = usuario;
    }

    public List<Usuario> ObtenerUsuarios()
    {
        return _listaDeUsuarios;
    }

    public void AgregarEquipo(Equipo equipo)
    {
        _listaDeEquipos.Add(equipo);
    }

    public void BorrarEquipo(Equipo equipo)
    {
        _listaDeEquipos.Remove(equipo);
    }

    public void AgregarEstadio(Estadio estadio)
    {
        _listaDeEstadios.Add(estadio);
    }

    public void BorrarEstadio(Estadio estadio)
    {
        _listaDeEstadios.Remove(estadio);
    }

    public void AgregarPartido(Partido partido)
    {
        _listaDePartidos.Add(partido);
    }

    public void BorrarPartido(Partido partido)
    {
        _listaDePartidos.Remove(partido);
    }
}