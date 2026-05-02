using Dominio.Clases;
namespace Servicios.Interfaces;

public interface IServicioPosicionEquipo
{
    void AgregarPosicion(PosicionEquipo posicion);

    void ActualizarPosicion(PosicionEquipo posicion);

    void EliminarPosicion(PosicionEquipo posicion);

    List<PosicionEquipo> ObtenerPosiciones();

    PosicionEquipo ObtenerPosicionPorEquipo(string nombreEquipo);
}