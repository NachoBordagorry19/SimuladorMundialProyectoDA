using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioEstadio
{
    void AgregarEstadio(EstadioDTO estadioDTO);
    List<EstadioDTO> ObtenerEstadios();
    EstadioDTO ObtenerEstadioPorNombre(string nombre);
    void EliminarEstadio(EstadioDTO estadioDTO);
    void ActualizarEstadio(EstadioDTO estadioDTO);
    void ActualizarEstadio(string nombreOriginal, EstadioDTO estadioDTO);
}