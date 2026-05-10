using Dominio.Clases;
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
        if (string.IsNullOrEmpty(dto.mensaje))
        {
            throw new ArgumentException("El mensaje no puede ser vacío");
        }
        if (string.IsNullOrEmpty(dto.usuario))
        {
            throw new ArgumentException("El usuario no puede ser nulo");
        }
        Log nuevoLog = new Log();
        nuevoLog.Mensaje = dto.mensaje;
        nuevoLog.Usuario = dto.usuario;
        nuevoLog.Tipo = dto.tipo;
        
        if (string.IsNullOrEmpty(dto.fechaISO8601))
        {
            nuevoLog.Fecha = DateTime.Now;
        }
        else
        {
            nuevoLog.Fecha = DateTime.Parse(dto.fechaISO8601);
        }

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
}