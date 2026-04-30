using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class EquipoServicios
{
    private readonly IEquipoRepositorio _equipoRepositorio;

    public EquipoServicios(IEquipoRepositorio equipoRepositorio)
    {
        _equipoRepositorio = equipoRepositorio;
    }

    public void AgregarEquipo(EquipoDTO equipoDTO)
    {
        Equipo equipo = EquipoDTOAEntidad(equipoDTO);
        _equipoRepositorio.AgregarEquipo(equipo);
    }

    public Equipo EquipoDTOAEntidad(EquipoDTO equipoDto)
    {
        Equipo equipo = new Equipo()
        {
            Nombre = equipoDto.nombre,
            Confederacion = equipoDto.confederacion,
            RankingFifa = equipoDto.rankingFifa,
        };
        return equipo;
    }
    
    
}