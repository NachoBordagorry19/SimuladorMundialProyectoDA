using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios.Clases;

public class FixturePrimeraFaseServicio
{
    private readonly IEquipoRepositorio _equipoRepositorio;
    private readonly IEstadioRepositorio _estadioRepositorio;
    private readonly IPartidoRepositorio _partidoRepositorio;
    private readonly EquipoServicios _equipoServicios;
    private readonly EstadioServicios _estadioServicios;

    public FixturePrimeraFaseServicio(
        IEquipoRepositorio equipoRepositorio,
        IEstadioRepositorio estadioRepositorio,
        IPartidoRepositorio partidoRepositorio,
        EquipoServicios equipoServicios,
        EstadioServicios estadioServicios)
    {
        _equipoRepositorio = equipoRepositorio;
        _estadioRepositorio = estadioRepositorio;
        _partidoRepositorio = partidoRepositorio;
        _equipoServicios = equipoServicios;
        _estadioServicios = estadioServicios;
    }

    public ResultadoFixture GenerarFixturePrimeraFase(int semillaFixture, DateTime? fechaInicio = null)
    {
        var resultado = new ResultadoFixture
        {
            SemillaFixture = semillaFixture,
            FechaGeneracion = DateTime.UtcNow,
            EquiposOrdenados = new List<EquipoDTO>(),
            AuditoriaEmpates = new List<EntradaAuditoria>()
        };

        return resultado;
    }
}