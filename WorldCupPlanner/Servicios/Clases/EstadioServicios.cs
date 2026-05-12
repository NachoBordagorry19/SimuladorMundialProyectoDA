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
        ValidarNombreNoExiste(estadioDTO.Nombre);
        Estadio estadio = EstadioDTOAEntidad(estadioDTO);
        _estadioRepositorio.AgregarEstadio(estadio);
    }

    private void ValidarNombreNoExiste(string nombre)
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

    public List<EstadioDTO> ObtenerEstadios()
    {
        List<EstadioDTO> estadiosDTO = new List<EstadioDTO>();
        foreach (var estadio in _estadioRepositorio.ObtenerEstadios())
        {
            estadiosDTO.Add(DesdeEntidad(estadio));
        }
        return estadiosDTO;
    }

    public EstadioDTO ObtenerEstadioPorNombre(string nombre)
    {
        Estadio? estadio = _estadioRepositorio.ObtenerEstadioPorNombre(nombre);
        if (estadio == null)
        {
            throw new ArgumentException("El estadio no existe");
        }
        return DesdeEntidad(estadio);
    }

    public void EliminarEstadio(EstadioDTO estadioDTO)
    {
        Estadio? estadioExistente = _estadioRepositorio.ObtenerEstadioPorNombre(estadioDTO.Nombre);
        if (estadioExistente == null)
        {
            throw new ArgumentException("El estadio no existe");
        }
        _estadioRepositorio.EliminarEstadio(estadioExistente);
    }

    public void ActualizarEstadio(EstadioDTO estadioDTO)
    {
        Estadio? existente = _estadioRepositorio.ObtenerEstadioPorNombre(estadioDTO.Nombre);
        if (existente == null)
        {
            throw new ArgumentException("El estadio no existe");
        }
        Estadio estadio = EstadioDTOAEntidad(estadioDTO);
        _estadioRepositorio.ActualizarEstadio(estadio);
    }

    private EstadioDTO DesdeEntidad(Estadio estadio)
    {
        return new EstadioDTO()
        {
            Nombre = estadio.Nombre,
            Ciudad = estadio.Ciudad,
            Descripcion = estadio.Descripcion,
            CapacidadLocativa = estadio.CapacidadLocativa
        };
    }
}