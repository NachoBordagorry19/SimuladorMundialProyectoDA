using Dominio.Clases;

namespace Servicios.Interfaces;

public interface IImportadorEquiposCSV
{ 
    List<Equipo> ImportarDesdeCSV(string rutaArchivo);
}
