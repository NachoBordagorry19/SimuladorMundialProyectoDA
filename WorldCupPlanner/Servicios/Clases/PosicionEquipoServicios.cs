using System;
using System.Linq;
using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Modelo;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class PosicionEquipoServicios : IServicioPosicionEquipo
{
    private readonly IPosicionEquipoRepositorio _posicionEquipoRepositorio;

    public PosicionEquipoServicios(IPosicionEquipoRepositorio posicionEquipoRepositorio)
    {
        _posicionEquipoRepositorio = posicionEquipoRepositorio;
    }

    public void AgregarPosicion(PosicionEquipoDTO posicionEquipoDTO)
    {
        throw new NotImplementedException();
    }

    public void ActualizarPosicion(PosicionEquipoDTO posicionEquipoDTO)
    {
        throw new NotImplementedException();
    }

    public void EliminarPosicion(PosicionEquipoDTO posicionEquipoDTO)
    {
        throw new NotImplementedException();
    }

    public List<PosicionEquipoDTO> ObtenerPosiciones()
    {
        throw new NotImplementedException();
    }

    public PosicionEquipo ObtenerPosicionPorEquipo(string nombreEquipo)
    {
        throw new NotImplementedException();
    }
}