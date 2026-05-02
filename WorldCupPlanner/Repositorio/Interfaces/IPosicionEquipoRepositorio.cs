using Dominio.Clases;
namespace Repositorio.Interfaces;

public interface IPosicionEquipoRepositorio
{
    List<PosicionEquipo> ObtenerPosiciones();
    void AgregarPosicion(PosicionEquipo posicion);
    PosicionEquipo? ObtenerPosicionPorEquipo(string nombreEquipo);
    bool ActualizarPosicion(PosicionEquipo posicion);
    void EliminarPosicion(PosicionEquipo posicion);
}