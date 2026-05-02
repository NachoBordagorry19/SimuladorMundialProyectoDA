namespace Repositorio.Interfaces;

public interface IPosicionEquipoRepositorio
{
    void Agregar(PosicionEquipoDTO posicion);

    void Actualizar(PosicionEquipoDTO posicion);

    void Eliminar(PosicionEquipoDTO posicion);

    List<PosicionEquipoDTO> ObtenerTodos();

    PosicionEquipoDTO ObtenerPorNombreEquipo(string nombreEquipo);
}