using System.Text;
using ClosedXML.Excel;
using Dominio.Clases;
using Repositorio.Interfaces;
using Servicios.Interfaces;

namespace Servicios.Clases;

public class ServicioExportacion : IServicioExportacion
{
    private readonly IAuditoriaRepositorio _auditoriaRepo;

    public ServicioExportacion(IAuditoriaRepositorio auditoriaRepo)
    {
        _auditoriaRepo = auditoriaRepo;
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
}