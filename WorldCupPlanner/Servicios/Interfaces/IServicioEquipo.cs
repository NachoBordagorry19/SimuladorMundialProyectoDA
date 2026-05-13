using Dominio.Enums;
using Dominio.Clases;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioEquipo
{
    void AgregarEquipo(EquipoDTO equipoDTO);
    List<EquipoDTO> ObtenerEquipos();
    List<String> GenerarEquiposAutomaticamente(int semillaCompletar);
    int ObtenerCupo(Confederacion confederacion);

    void ActualizarEquipo(EquipoDTO equipoDTO);
    void ActualizarEquipo(string nombreOriginal, EquipoDTO equipoDTO);

    EquipoDTO ObtenerEquipo(string nombre);

    void EliminarEquipo(EquipoDTO equipoDTO);

    ResultadoFixture ResolverEmpatesYOrdenar(List<EquipoDTO> equipos, int semillaFixture);
    Equipo EquipoDTOAEntidad(EquipoDTO equipoDto);
    EquipoDTO EquipoEntidadAEquipoDTO(Equipo equipo);
}