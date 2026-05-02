using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class EquipoRepositorio : IEquipoRepositorio
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;

    public EquipoRepositorio(BaseDeDatosEnMemoria baseDeDatosEnMemoria)
    {
        _baseDeDatosEnMemoria = baseDeDatosEnMemoria;
    }

    public List<Equipo> ObtenerEquipos()
    {
        return _baseDeDatosEnMemoria.ObtenerEquipos();
    }

    public void AgregarEquipo(Equipo equipo)
    {
        _baseDeDatosEnMemoria.AgregarEquipo(equipo);
    }

    public Equipo? ObtenerEquipo(Func<Equipo, bool> filtro)
    {
        return _baseDeDatosEnMemoria.ObtenerEquipos().Where(filtro).FirstOrDefault();
    }

    public void EliminarEquipo(Equipo equipo)
    {
        _baseDeDatosEnMemoria.BorrarEquipo(equipo);
    }

    public void ActualizarEquipo(Equipo equipo)
    {
        _baseDeDatosEnMemoria.ActualizarEquipo(equipo);
    }
}