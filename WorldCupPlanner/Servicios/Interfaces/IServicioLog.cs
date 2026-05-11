using Dominio.Enums;
using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioLog
{
    void RegistrarAltaEquipo(string nombre, string confederacion);
    void RegistrarEdicionEquipo(string nombre);
    void RegistrarEliminacionEquipo(string nombre);
}