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

    public void AgregarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }

    public void ActualizarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }

    public void EliminarPosicion(PosicionEquipo posicion)
    {
        throw new NotImplementedException();
    }

    public List<PosicionEquipo> ObtenerPosiciones()
    {
        throw new NotImplementedException();
    }

    public PosicionEquipo ObtenerPosicionPorEquipo(string nombreEquipo)
    {
        throw new NotImplementedException();
    }
}