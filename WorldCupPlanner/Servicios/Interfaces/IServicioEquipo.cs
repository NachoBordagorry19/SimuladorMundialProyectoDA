using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioEquipo
{
    void AgregarEquipo(EquipoDTO equipoDTO);
    List<EquipoDTO> ObtenerEquipos();
    void GenerarEquiposAutomaticamente(int semillaCompletar);
    int ObtenerCupo(Confederacion confederacion);
}