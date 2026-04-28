using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioEstadio
{
    void AgregarEstadio(EstadioDTO estadioDTO);
    List<EstadioDTO> ObtenerEstadios();
    EstadioDTO ObtenerEstadioPorNombre(string nombre);
    void EliminarEstadio(EstadioDTO estadioDTO);
}