using Dominio.Clases;
using Repositorio.Interfaces;

namespace TestServicios;

public class AuditoriaRepositorioMock : IAuditoriaRepositorio
{
    private readonly List<Auditoria> _datos = new();

    public void AgregarRegistro(Auditoria registro)
    {
        _datos.Add(registro);
    }
    public List<Auditoria> ObtenerTodosLosRegistros()
    {
        return _datos;
    }

    public List<Auditoria> ObtenerPorRango(DateTime desde, DateTime hasta)
    {
        return _datos.Where(a => a.FechaHora >= desde && a.FechaHora <= hasta).ToList();
    }
}