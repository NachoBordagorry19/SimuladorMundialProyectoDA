using Dominio.Clases;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Modelo;
using System.Collections.Generic;
using Dominio.Enums;

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
        int cupo = ObtenerCupo(equipoDTO.confederacion);
        if (cupo > 0)
        {
            var cantidadActual = _equipoRepositorio.ObtenerEquipos().Count(e => e.Confederacion == equipoDTO.confederacion);
            if (cantidadActual >= cupo)
            {
                throw new ArgumentException($"Cupo máximo alcanzado para la confederación {equipoDTO.confederacion}");
            }
        }
        ValidarNombreNoExiste(equipoDTO.nombre);
        Equipo equipo = EquipoDTOAEntidad(equipoDTO);
        _equipoRepositorio.AgregarEquipo(equipo);
    }
    
    private int ObtenerCupo(Confederacion confederacion)
    {
        switch (confederacion)
        {
            case Confederacion.UEFA: return 16;
            case Confederacion.CONMEBOL: return 7;
            case Confederacion.CONCACAF: return 7;
            case Confederacion.CAF: return 9;
            case Confederacion.AFC: return 8;
            case Confederacion.OFC: return 1;
            default: return 0;
        }
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
        ValidarNombreExiste(nombre);
        Equipo? equipo = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        return EquipoEntidadAEquipoDTO(equipo);
    }

    public void ValidarNombreExiste(string nombre)
    {
        Equipo equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipoExistente == null)
        {
            throw new ArgumentException("El equipo a buscar no existe por favor ingrese uno que exista");
        }
    }

    public void ValidarNombreNoExiste(string nombre)
    {
        Equipo equipoNoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipoNoExistente != null)
        {
            throw new ArgumentException("El equipo a agregar ya existe porfavor ingrese otro");
        }
    }

    public void EliminarEquipo(EquipoDTO equipoDto)
    {
        ValidarNombreExiste(equipoDto.nombre);
        Equipo? equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == equipoDto.nombre);
        _equipoRepositorio.EliminarEquipo(equipoExistente);
    }

    public void ActualizarEquipo(EquipoDTO equipoDto)
    {
        ValidarNombreExiste(equipoDto.nombre);
        Equipo equipoActualizado = EquipoDTOAEntidad(equipoDto);
        _equipoRepositorio.ActualizarEquipo(equipoActualizado);
    }

}