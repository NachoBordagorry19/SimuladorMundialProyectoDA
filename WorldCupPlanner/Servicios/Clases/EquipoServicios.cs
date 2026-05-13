using Dominio.Clases;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Modelo;
using System.Collections.Generic;
using Dominio.Enums;
using System;
using System.Linq;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class EquipoServicios : IServicioEquipo
{
    private readonly IEquipoRepositorio _equipoRepositorio;
    private readonly IServicioAuditoria _auditoria;

    public EquipoServicios(IEquipoRepositorio equipoRepo, IServicioAuditoria auditoria)
    {
        _equipoRepositorio = equipoRepo;
        _auditoria = auditoria;
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
        _auditoria.RegistrarAltaEquipo(equipoDTO.nombre);
    }

    public void GenerarEquiposAutomaticamente(int semillaCompletar)
{
    Random random = new Random(semillaCompletar);
    Array valoresEnum = Enum.GetValues(typeof(Confederacion));

    List<EquipoDTO> todosLosExistentes = ObtenerEquipos();
    
    if (todosLosExistentes.Count >= 48)
    {
        return;
    }

    foreach (Confederacion conf in valoresEnum)
    {
        int cupoMaximo = ObtenerCupo(conf);
        List<EquipoDTO> todosLosEquipos = ObtenerEquipos();
        
        int cantidadActual = 0;
        foreach (EquipoDTO equipo in todosLosEquipos)
        {
            if (equipo.confederacion == conf)
            {
                cantidadActual++;
            }
        }

        int faltantes = cupoMaximo - cantidadActual;

        for (int i = 1; i <= faltantes; i++)
        {
            int numeroEquipo = cantidadActual + i;
            string nombreFormateado = conf.ToString() + "_" + numeroEquipo.ToString("D2");
            
            EquipoDTO nuevoEquipo = new EquipoDTO();
            nuevoEquipo.nombre = nombreFormateado;
            nuevoEquipo.confederacion = conf;
            nuevoEquipo.rankingFifa = random.Next(300, 2501);

            this.AgregarEquipo(nuevoEquipo);
        }
    }

    _auditoria.RegistrarGeneracionAutomaticaEquipos(48);
}
    public int ObtenerCupo(Confederacion confederacion)
    {
        switch (confederacion)
        {
            case Confederacion.UEFA:
                return 16;
            case Confederacion.CONMEBOL:
                return 7;
            case Confederacion.CONCACAF:
                return 7;
            case Confederacion.CAF:
                return 9;
            case Confederacion.AFC:
                return 8;
            case Confederacion.OFC:
                return 1;
            default:
                return 0;
        }
    }

    public Equipo EquipoDTOAEntidad(EquipoDTO equipoDto)
    {
        return new Equipo(
            equipoDto.nombre,
            equipoDto.confederacion,
            equipoDto.rankingFifa
        );
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

    public EquipoDTO EquipoEntidadAEquipoDTO(Equipo equipo)
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
        _auditoria.RegistrarEliminacionEquipo(equipoDto.nombre);
    }

    public void ActualizarEquipo(EquipoDTO equipoDto)
    {
        ActualizarEquipo(equipoDto.nombre, equipoDto);
    }

    public void ActualizarEquipo(string nombreOriginal, EquipoDTO equipoDto)
    {
        ValidarNombreExiste(nombreOriginal);

        Equipo equipoOriginal = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombreOriginal);

        if (nombreOriginal != equipoDto.nombre)
        {
            ValidarNombreNoExiste(equipoDto.nombre);
        }

        if (equipoOriginal.Confederacion != equipoDto.confederacion)
        {
            int cupo = ObtenerCupo(equipoDto.confederacion);
            int cantidadActual = _equipoRepositorio.ObtenerEquipos()
                .Count(e => e.Confederacion == equipoDto.confederacion);

            if (cantidadActual >= cupo)
            {
                throw new ArgumentException($"Cupo máximo alcanzado para la confederación {equipoDto.confederacion}");
            }
        }

        Equipo equipoActualizado = EquipoDTOAEntidad(equipoDto);

        _equipoRepositorio.EliminarEquipo(equipoOriginal);
        _equipoRepositorio.AgregarEquipo(equipoActualizado);
        _auditoria.RegistrarEdicionEquipo(equipoDto.nombre);
    }

    public ResultadoFixture ResolverEmpatesYOrdenar(List<EquipoDTO> equipos, int semillaFixture)
    {
        VerificarListaDeEquiposSinNulo(equipos);

        var ordenBase = OrdenoPorRankinFifa(equipos);

        var auditoria = new List<EntradaAuditoria>();
        var listaFinal = new List<EquipoDTO>();

        var generadorDeNumerosPrincipal = new Random(semillaFixture);

        var grupos = ordenBase.GroupBy(e => e.rankingFifa).OrderByDescending(g => g.Key);
        foreach (var grupo in grupos)
        {
            var listaGrupo = grupo.ToList();
            if (listaGrupo.Count <= 1)
            {
                listaFinal.AddRange(listaGrupo);
                continue;
            }

            var ordenOriginal = listaGrupo.Select(x => x.nombre).ToList();

            int semillaGrupo = generadorDeNumerosPrincipal.Next();
            var generadorDeNumerosGrupo = new Random(semillaGrupo);

            var copiaGrupo = listaGrupo.ToList();
            for (int i = copiaGrupo.Count - 1; i > 0; i--)
            {
                int j = generadorDeNumerosGrupo.Next(i + 1);
                var copiaTemporal = copiaGrupo[i];
                copiaGrupo[i] = copiaGrupo[j];
                copiaGrupo[j] = copiaTemporal;
            }

            var ordenResuelto = copiaGrupo.Select(x => x.nombre).ToList();

            auditoria.Add(new EntradaAuditoria
            {
                RankingFifa = grupo.Key,
                OrdenOriginal = ordenOriginal,
                OrdenResuelto = ordenResuelto,
                SemillaUsada = semillaGrupo,
                Nota = "Empate resuelto con Fisher–Yates derivando semilla desde SemillaFixture"
            });

            listaFinal.AddRange(copiaGrupo);
        }

        var resultado = new ResultadoFixture
        {
            SemillaFixture = semillaFixture,
            FechaGeneracion = DateTime.UtcNow,
            EquiposOrdenados = listaFinal,
            AuditoriaEmpates = auditoria
        };

        return resultado;
    }


    public void VerificarListaDeEquiposSinNulo(List<EquipoDTO> equipos)
    {
        for (int i = 0; i < equipos.Count; i++)
        {
            if (equipos[i] == null)
            {
                throw new ArgumentException("La lista de equipos contiene elementos nulos");
            }
        }
    }

    public List<EquipoDTO> OrdenoPorRankinFifa(List<EquipoDTO> equipos)
    {
        if (equipos == null) return new List<EquipoDTO>();
        var ordenBase = equipos
            .OrderByDescending(e => e.rankingFifa)
            .ThenBy(e => e.nombre, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return ordenBase;
    }


}
