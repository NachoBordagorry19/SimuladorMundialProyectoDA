namespace Servicios.Interfaces;

public interface IServicioPosicionEquipo
{
    void AgregarPosicion(PosicionEquipoDTO posicion);

    void ActualizarPosicion(PosicionEquipoDTO posicion);

    void EliminarPosicion(PosicionEquipoDTO posicion);

    List<PosicionEquipoDTO> ObtenerPosiciones();

    PosicionEquipoDTO ObtenerPosicionPorEquipo(string nombreEquipo);
}