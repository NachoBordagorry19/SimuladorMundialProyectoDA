using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IEstadioRepositorio
{
    List<Estadio> ObtenerEstadios();
    void AgregarEstadio(Estadio estadio);
    Estadio? ObtenerEstadioPorNombre(string nombre);
    bool ActualizarEstadio(Estadio estadio);
    void EliminarEstadio(Estadio estadio);
}