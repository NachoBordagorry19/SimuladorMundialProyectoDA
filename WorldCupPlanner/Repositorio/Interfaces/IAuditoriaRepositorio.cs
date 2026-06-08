using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IAuditoriaRepositorio
{
    void AgregarRegistro(Auditoria registro);
    List<Auditoria> ObtenerTodosLosRegistros();
    List<Auditoria> ObtenerPorRango(DateTime desde, DateTime hasta);

}