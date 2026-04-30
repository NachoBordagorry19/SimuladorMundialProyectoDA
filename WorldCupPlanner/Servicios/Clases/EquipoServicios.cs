using Dominio.Clases;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Modelo;
using System.Collections.Generic;

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

    public List<EquipoDTO> ObtenerEquipos()
    {
        List<Equipo> equipos = _equipoRepositorio.ObtenerEquipos();
        List<EquipoDTO> equiposDTOS = new List<EquipoDTO>();
        foreach (var equipo in equipos)
        {
            EquipoDTO equipoDto = EquipoEntidadAEquipoDTO(equipo);
            equiposDTOS.Add(equipoDto);
        }

        return equiposDTOS;
    }

    public static EquipoDTO EquipoEntidadAEquipoDTO(Equipo equipo)
    {
        EquipoDTO equipoDto = new EquipoDTO()
        {
            nombre = equipo.Nombre,
            confederacion = equipo.Confederacion,
            rankingFifa = equipo.RankingFifa
        };
        return equipoDto;
    }

    public EquipoDTO ObtenerEquipo(string nombre)
    {
        ValidarNombre(nombre);
        Equipo? equipo = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        return EquipoEntidadAEquipoDTO(equipo);
    }

    public void ValidarNombre(string nombre)
    {
        Equipo equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipoExistente == null)
        {
            throw new ArgumentException("El equipo a buscar no existe por favor ingrese uno que exista");
        }
    }

    public void EliminarEquipo(EquipoDTO equipoDto)
    {
        ValidarNombre(equipoDto.nombre);
        Equipo? equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == equipoDto.nombre);
        _equipoRepositorio.EliminarEquipo(equipoExistente);
    }

    public void ActualizarEquipo(EquipoDTO equipoDto)
    {
        Equipo equipoActualizado = EquipoDTOAEntidad(equipoDto);
        _equipoRepositorio.ActualizarEquipo(equipoActualizado);
    }

}