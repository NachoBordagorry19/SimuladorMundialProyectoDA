using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IEquipoRepositorio
{
    public List<Equipo> ObtenerEquipos();
    public void AgregarEquipo(Equipo equipo);
    public Equipo? ObtenerEquipo(Func<Equipo, bool> filtro);
    public void EliminarEquipo(Equipo equipo);
    public void ActualizarEquipo(Equipo equipo);
}