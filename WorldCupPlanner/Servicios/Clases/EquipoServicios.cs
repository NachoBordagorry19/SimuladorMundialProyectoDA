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
        int cupo = ObtenerCupo(equipoDTO.Confederacion);
        if (cupo > 0)
        {
            int cantidadActual = _equipoRepositorio.ObtenerEquipos().Count(e => e.Confederacion == equipoDTO.Confederacion);
            if (cantidadActual >= cupo)
            {
                throw new ArgumentException($"Cupo máximo alcanzado para la confederación {equipoDTO.Confederacion}");
            }
        }
        ValidarNombreNoExiste(equipoDTO.Nombre);
        Equipo equipo = EquipoDTOAEntidad(equipoDTO);
        _equipoRepositorio.AgregarEquipo(equipo);
        _auditoria.RegistrarAltaEquipo(equipoDTO.Nombre);
    }

    public void GenerarEquiposAutomaticamente(int semillaCompletar)
    {
        Random random = new Random(semillaCompletar);
        Array valoresEnum = Enum.GetValues(typeof(Confederacion));

        List<EquipoDTO> todosLosExistentes = ObtenerEquipos();

        int equiposACompletar = 48;
        if (todosLosExistentes.Count >= equiposACompletar)
        {
            return;
        }

        var desglosePorConfederacion = new List<string>();

        foreach (Confederacion conf in valoresEnum)
        {
            int cupoMaximo = ObtenerCupo(conf);
            List<EquipoDTO> todosLosEquipos = ObtenerEquipos();

            int cantidadActual = 0;
            foreach (EquipoDTO equipo in todosLosEquipos)
            {
                if (equipo.Confederacion == conf)
                {
                    cantidadActual++;
                }
            }

            int faltantes = cupoMaximo - cantidadActual;

            if (faltantes > 0)
                desglosePorConfederacion.Add($"{conf}:{faltantes}");

            for (int i = 1; i <= faltantes; i++)
            {
                int numeroEquipo = cantidadActual + i;
                string nombreFormateado = conf.ToString() + "_" + numeroEquipo.ToString("D2");

                EquipoDTO nuevoEquipo = new EquipoDTO();
                nuevoEquipo.Nombre = nombreFormateado;
                nuevoEquipo.Confederacion = conf;
                nuevoEquipo.RankingFifa = random.Next(300, 2501);

                this.AgregarEquipo(nuevoEquipo);
            }
        }

        int rankingFifaMinimo = 300;
        int rankingFifaMaximo = 2500;
        string desgloseTexto = string.Join(", ", desglosePorConfederacion);
        _auditoria.RegistrarGeneracionAutomaticaEquipos(equiposACompletar, semillaCompletar, rankingFifaMinimo, rankingFifaMaximo, desgloseTexto);
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
        var equipo = new Equipo(
            equipoDto.Nombre,
            equipoDto.Confederacion,
            equipoDto.RankingFifa
        );
        equipo.BanderaBase64 = equipoDto.BanderaBase64;
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

    public EquipoDTO EquipoEntidadAEquipoDTO(Equipo equipo)
    {
        return new EquipoDTO()
        {
            Nombre = equipo.Nombre,
            Confederacion = equipo.Confederacion,
            RankingFifa = equipo.RankingFifa,
            BanderaBase64 = equipo.BanderaBase64
        };
    }


    public EquipoDTO ObtenerEquipo(string nombre)
    {
        ValidarNombreExiste(nombre);
        Equipo? equipo = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipo == null) throw new ArgumentException("El equipo a consultar no existe");
        return EquipoEntidadAEquipoDTO(equipo);
    }

    public void ValidarNombreExiste(string nombre)
    {
        Equipo? equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipoExistente == null)
        {
            throw new ArgumentException("El equipo a buscar no existe por favor ingrese uno que exista");
        }
    }

    public void ValidarNombreNoExiste(string nombre)
    {
        Equipo? equipoNoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombre);
        if (equipoNoExistente != null)
        {
            throw new ArgumentException("El equipo a agregar ya existe por favor ingrese otro");
        }
    }

    public void EliminarEquipo(EquipoDTO equipoDto)
    {
        ValidarNombreExiste(equipoDto.Nombre);
        Equipo? equipoExistente = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == equipoDto.Nombre);
        if (equipoExistente == null) throw new ArgumentException("El equipo a eliminar no existe");
        _equipoRepositorio.EliminarEquipo(equipoExistente);
        _auditoria.RegistrarEliminacionEquipo(equipoDto.Nombre);
    }

    public void ActualizarEquipo(EquipoDTO equipoDto)
    {
        ActualizarEquipo(equipoDto.Nombre, equipoDto);
    }

    public void ActualizarEquipo(string nombreOriginal, EquipoDTO equipoDto)
    {
        ValidarNombreExiste(nombreOriginal);

        Equipo? equipoOriginal = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == nombreOriginal);
        if (equipoOriginal == null) throw new ArgumentException("El equipo original no existe");

        if (nombreOriginal != equipoDto.Nombre)
        {
            ValidarNombreNoExiste(equipoDto.Nombre);
        }

        if (equipoOriginal.Confederacion != equipoDto.Confederacion)
        {
            int cupo = ObtenerCupo(equipoDto.Confederacion);
            int cantidadActual = _equipoRepositorio.ObtenerEquipos()
                .Count(e => e.Confederacion == equipoDto.Confederacion);

            if (cantidadActual >= cupo)
            {
                throw new ArgumentException($"Cupo máximo alcanzado para la confederación {equipoDto.Confederacion}");
            }
        }
        
        equipoOriginal.Nombre = equipoDto.Nombre;
        equipoOriginal.Confederacion = equipoDto.Confederacion;
        equipoOriginal.RankingFifa = equipoDto.RankingFifa;
        equipoOriginal.BanderaBase64 = equipoDto.BanderaBase64;
        _equipoRepositorio.ActualizarEquipo(equipoOriginal);
        _auditoria.RegistrarEdicionEquipo(equipoDto.Nombre);
    }

    public ResultadoFixture ResolverEmpatesYOrdenar(List<EquipoDTO> equipos, int semillaFixture)
    {
        VerificarListaDeEquiposSinNulo(equipos);

        List<EquipoDTO> ordenBase = OrdenarPorRankingFifa(equipos);

        var auditoria = new List<EntradaAuditoria>();
        var listaFinal = new List<EquipoDTO>();

        var generadorDeNumerosPrincipal = new Random(semillaFixture);

        var grupos = ordenBase.GroupBy(e => e.RankingFifa).OrderByDescending(g => g.Key);
        foreach (var grupo in grupos)
        {
            List<EquipoDTO> listaGrupo = grupo.ToList();
            if (listaGrupo.Count <= 1)
            {
                listaFinal.AddRange(listaGrupo);
                continue;
            }

            var ordenOriginal = listaGrupo.Select(x => x.Nombre).ToList();

            int semillaGrupo = generadorDeNumerosPrincipal.Next();
            var generadorDeNumerosGrupo = new Random(semillaGrupo);

            List<EquipoDTO> copiaGrupo = listaGrupo.ToList();
            for (int i = copiaGrupo.Count - 1; i > 0; i--)
            {
                int j = generadorDeNumerosGrupo.Next(i + 1);
                var copiaTemporal = copiaGrupo[i];
                copiaGrupo[i] = copiaGrupo[j];
                copiaGrupo[j] = copiaTemporal;
            }

            var ordenResuelto = copiaGrupo.Select(e => e.Nombre).ToList();

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

    public List<EquipoDTO> OrdenarPorRankingFifa(List<EquipoDTO> equipos)
    {
        if (equipos == null) return new List<EquipoDTO>();
        List<EquipoDTO> ordenBase = equipos
            .OrderByDescending(e => e.RankingFifa)
            .ThenBy(e => e.Nombre, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return ordenBase;
    }


}
