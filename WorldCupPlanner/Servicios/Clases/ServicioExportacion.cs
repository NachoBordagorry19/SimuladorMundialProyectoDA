using System.Text;
using ClosedXML.Excel;
using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class ServicioExportacion : IServicioExportacion
{
    private readonly IAuditoriaRepositorio _auditoriaRepo;
    private readonly IServicioPartido _servicioPartido;

    public ServicioExportacion(IAuditoriaRepositorio auditoriaRepo, IServicioPartido servicioPartido)
    {
        _auditoriaRepo = auditoriaRepo;
        _servicioPartido = servicioPartido;
    }

    public byte[] ExportarAuditoriaCsv(DateTime desde, DateTime hasta)
    {
        List<Auditoria> registros = _auditoriaRepo.ObtenerPorRango(desde, hasta);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("FechaHora,Usuario,Accion,Detalle");
        foreach (Auditoria r in registros)
            sb.AppendLine($"{r.FechaHora:yyyy-MM-dd HH:mm:ss},{r.Usuario},{r.Accion},{r.Detalle}");
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] ExportarAuditoriaXlsx(DateTime desde, DateTime hasta)
    {
        List<Auditoria> registros = _auditoriaRepo.ObtenerPorRango(desde, hasta);
        using XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet hoja = workbook.Worksheets.Add("Auditoria");
        hoja.Cell(1, 1).Value = "FechaHora";
        hoja.Cell(1, 2).Value = "Usuario";
        hoja.Cell(1, 3).Value = "Accion";
        hoja.Cell(1, 4).Value = "Detalle";
        for (int i = 0; i < registros.Count; i++)
        {
            Auditoria r = registros[i];
            hoja.Cell(i + 2, 1).Value = r.FechaHora.ToString("yyyy-MM-dd HH:mm:ss");
            hoja.Cell(i + 2, 2).Value = r.Usuario;
            hoja.Cell(i + 2, 3).Value = r.Accion;
            hoja.Cell(i + 2, 4).Value = r.Detalle;
        }
        using MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }

    public byte[] ExportarFixtureCsv()
    {
        List<PartidoDTO> partidos = _servicioPartido.ObtenerPartidos()
            .Where(p => p.EstadoPartido == EstadoPartido.Jugado)
            .ToList();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Fecha,Fase,EquipoLocal,GolesLocal,GolesVisitante,EquipoVisitante,Estadio");
        foreach (PartidoDTO p in partidos)
            sb.AppendLine($"{p.Fecha:yyyy-MM-dd},{p.Fase},{p.EquipoLocal.Nombre},{p.GolesLocal},{p.GolesVisitante},{p.EquipoVisitante.Nombre},{p.Estadio.Nombre}");
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] ExportarFixtureXlsx()
    {
        List<PartidoDTO> partidos = _servicioPartido.ObtenerPartidos()
            .Where(p => p.EstadoPartido == EstadoPartido.Jugado)
            .ToList();
        using XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet hoja = workbook.Worksheets.Add("Fixture");
        hoja.Cell(1, 1).Value = "Fecha";
        hoja.Cell(1, 2).Value = "Fase";
        hoja.Cell(1, 3).Value = "EquipoLocal";
        hoja.Cell(1, 4).Value = "GolesLocal";
        hoja.Cell(1, 5).Value = "GolesVisitante";
        hoja.Cell(1, 6).Value = "EquipoVisitante";
        hoja.Cell(1, 7).Value = "Estadio";
        for (int i = 0; i < partidos.Count; i++)
        {
            PartidoDTO p = partidos[i];
            hoja.Cell(i + 2, 1).Value = p.Fecha.ToString("yyyy-MM-dd");
            hoja.Cell(i + 2, 2).Value = p.Fase.ToString();
            hoja.Cell(i + 2, 3).Value = p.EquipoLocal.Nombre;
            hoja.Cell(i + 2, 4).Value = p.GolesLocal;
            hoja.Cell(i + 2, 5).Value = p.GolesVisitante;
            hoja.Cell(i + 2, 6).Value = p.EquipoVisitante.Nombre;
            hoja.Cell(i + 2, 7).Value = p.Estadio.Nombre;
        }
        using MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }
}