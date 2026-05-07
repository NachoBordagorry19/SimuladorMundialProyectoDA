using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioEquipo
{
    void AgregarEquipo(EquipoDTO equipoDTO);
    List<EquipoDTO> ObtenerEquipos();
    void GenerarEquiposAutomaticamente();
    int ObtenerCupo(Confederacion confederacion);
}