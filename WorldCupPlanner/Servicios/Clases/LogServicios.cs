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
    
    public void RegistrarAltaEquipo(string nombre, string confederacion)
    {
        throw new NotImplementedException();
    }

    public void RegistrarEdicionEquipo(string nombre)
    {
        throw new NotImplementedException();
    }

    public void RegistrarEliminacionEquipo(string nombre)
    {
        throw new NotImplementedException();
    }
}