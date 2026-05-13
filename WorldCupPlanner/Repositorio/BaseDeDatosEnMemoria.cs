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
        if (usuario == null) throw new ArgumentNullException(nameof(usuario));
        int indiceUsuario = _listaDeUsuarios.FindIndex(u => u.Email == usuario.Email);
        if (indiceUsuario == -1) throw new ArgumentException("Usuario a actualizar no encontrado");
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
        if (equipo == null) throw new ArgumentNullException(nameof(equipo));
        int indiceEquipo = _listaDeEquipos.FindIndex(e => e.Nombre == equipo.Nombre);
        if (indiceEquipo == -1) throw new ArgumentException("Equipo a actualizar no encontrado");
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

    public void ActualizarPartido(Partido partido)
    {
        if (partido == null) throw new ArgumentNullException(nameof(partido));
        int indicePartido = _listaDePartidos.FindIndex(p => p.Id == partido.Id);
        if (indicePartido == -1) throw new ArgumentException("Partido a actualizar no encontrado");
        _listaDePartidos[indicePartido] = partido;
    }
}