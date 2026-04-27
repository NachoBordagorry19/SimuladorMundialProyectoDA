using System;
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

    public void ActualizarEquipo(Equipo equipo)
    {
        Equipo? equipoParaActualizar = _listaDeEquipos.Find(e => e.Nombre == equipo.Nombre);
        var indiceEquipo = _listaDeEquipos.IndexOf(equipoParaActualizar);
        _listaDeEquipos[indiceEquipo] = equipo;
    }

    public List<Equipo> ObtenerEquipos()
    {
        return _listaDeEquipos;
    }

    public void AgregarEstadio(Estadio estadio)
    {
        _listaDeEstadios.Add(estadio);
    }

    public void BorrarEstadio(Estadio estadio)
    {
        _listaDeEstadios.Remove(estadio);
    }


    public bool ActualizarEstadio(Estadio estadio)
    {
        if (estadio == null) throw new ArgumentNullException(nameof(estadio));
        int indice = _listaDeEstadios.FindIndex(e => e.Nombre == estadio.Nombre);
        if (indice == -1) return false;
        _listaDeEstadios[indice] = estadio;
        return true;
    }

    public List<Estadio> ObtenerEstadios()
    {
        return _listaDeEstadios;
    }

    public void AgregarPartido(Partido partido)
    {
        _listaDePartidos.Add(partido);
    }

    public void BorrarPartido(Partido partido)
    {
        _listaDePartidos.Remove(partido);
    }

    public List<Partido> ObtenerPartidos()
    {
        return _listaDePartidos;
    }
}