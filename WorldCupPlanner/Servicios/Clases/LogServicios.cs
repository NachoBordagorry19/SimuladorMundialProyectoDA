using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace Servicios.Clases;

public class LogServicios : IServicioLog
{
    private readonly ILogRepositorio _logRepo;

    public LogServicios(ILogRepositorio logRepo)
    {
        _logRepo = logRepo;
    }

    public void GuardarLog(LogDTO dto)
    {
        Log nuevoLog = new Log();
        nuevoLog.Mensaje = dto.mensaje;
        nuevoLog.Usuario = dto.usuario;
        nuevoLog.Tipo = dto.tipo;
        DateTime fechaParseada;
        bool esFechaValida = DateTime.TryParse(dto.fechaISO8601, out fechaParseada);

        if (esFechaValida)
        {
            nuevoLog.Fecha = fechaParseada;
        }
        else
        {
            nuevoLog.Fecha = DateTime.Now;
        }
        nuevoLog.Validar();
        _logRepo.AgregarLog(nuevoLog);
    }

    public List<LogDTO> ObtenerLogs()
    {
        List<Log> entidades = _logRepo.ObtenerLogs();
        List<LogDTO> listaDtos = new List<LogDTO>();

        foreach (Log log in entidades)
        {
            LogDTO dto = new LogDTO();
            dto.mensaje = log.Mensaje;
            dto.usuario = log.Usuario;
            dto.tipo = log.Tipo;
            dto.fechaISO8601 = log.Fecha.ToString("o");
        
            listaDtos.Add(dto);
        }

        return listaDtos;
    }
    
    public List<LogDTO> ObtenerLogsPorTipo(TipoLog tipo)
    {
        List<Log> logs = _logRepo.ObtenerLogsPorTipo(tipo);
    
        List<LogDTO> logsPorTipo = new List<LogDTO>();
        foreach (var log in logs)
        {
            logsPorTipo.Add(new LogDTO 
            { 
                mensaje = log.Mensaje, 
                usuario = log.Usuario, 
                tipo = log.Tipo, 
                fechaISO8601 = log.Fecha.ToString("o") 
            });
        }
        return logsPorTipo;
    }
}