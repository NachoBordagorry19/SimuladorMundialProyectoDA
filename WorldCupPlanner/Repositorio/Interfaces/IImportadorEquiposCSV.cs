using Dominio.Clases;

namespace Repositorio.Interfaces;

public interface IImportadorEquiposCSV
{
    List<Equipo> ImportarDesdeCSV(string rutaArchivo);
}

