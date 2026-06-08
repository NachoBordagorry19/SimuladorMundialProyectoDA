namespace Servicios.Interfaces;

public interface IServicioExportacion
{
    byte[] ExportarAuditoriaCsv(DateTime desde, DateTime hasta);
    byte[] ExportarAuditoriaXlsx(DateTime desde, DateTime hasta);
}