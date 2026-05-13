using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IAuditoriaRepositorio
{
    void AgregarRegistro(Auditoria registro);
    List<Auditoria> ObtenerTodosLosRegistros();

}