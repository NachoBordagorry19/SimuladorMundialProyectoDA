using System;
using System.Linq;
using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Modelo;
using Servicios.Interfaces;


namespace Servicios.Clases;

public class EstadioServicios : IServicioEstadio
{
    private readonly IEstadioRepositorio _estadioRepositorio;

    public EstadioServicios(IEstadioRepositorio estadioRepositorio)
    {
        _estadioRepositorio = estadioRepositorio;
    }

    public void AgregarEstadio(EstadioDTO estadioDTO)
    {
        Estadio estadio = EstadioDTOAEntidad(estadioDTO);
        NombreValido(estadio.Nombre);
        _estadioRepositorio.AgregarEstadio(estadio);
    }

    public void NombreValido(string nombre)
    {
        Estadio? estadioExiste = _estadioRepositorio.ObtenerEstadioPorNombre(nombre);
        if (estadioExiste != null)
        {
            throw new ArgumentException($"El estadio {nombre} ya existe");
        }
    }

    private Estadio EstadioDTOAEntidad(EstadioDTO estadioDTO)
    {
        var estadio = new Estadio(
            estadioDTO.Nombre,
            estadioDTO.Ciudad,
            estadioDTO.Descripcion,
            estadioDTO.CapacidadLocativa
        );

        return estadio;
    }
}