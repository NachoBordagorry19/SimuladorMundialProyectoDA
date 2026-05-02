using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioPosicionEquipo
{
    void AgregarPosicion(PosicionEquipoDTO posicionEquipoDTO);

    void ActualizarPosicion(PosicionEquipoDTO posicionEquipoDTO);

    void EliminarPosicion(PosicionEquipoDTO posicionEquipoDTO);

    List<PosicionEquipoDTO> ObtenerPosiciones();

    PosicionEquipoDTO ObtenerPosicionPorEquipo(string nombreEquipo);
}