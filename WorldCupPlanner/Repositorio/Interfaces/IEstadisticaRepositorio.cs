namespace Repositorio.Interfaces;

public interface IEstadisticaRepositorio
{
    EstadisticaEquipo? ObtenerPorEquipo(int equipoId);
    void Guardar(EstadisticaEquipo estadistica);
    IEnumerable<EstadisticaEquipo> ListarTodos();
    void EliminarPorEquipo(int equipoId);
    bool Existe(int equipoId);
}