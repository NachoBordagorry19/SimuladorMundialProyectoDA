using System;
using System.Collections.Generic;
using Dominio.Clases;

namespace Servicios.Modelo;

public class ResultadoFixture
{
    public List<EquipoDTO> EquiposOrdenados { get; set; } = new List<EquipoDTO>();

    public List<EntradaAuditoria> AuditoriaEmpates { get; set; } = new List<EntradaAuditoria>();

    public List<PartidoDTO> Partidos { get; set; } = new List<PartidoDTO>();

    public List<Grupo> Grupos { get; set; } = new List<Grupo>();

    public int SemillaFixture { get; set; }

    public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
}