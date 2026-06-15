namespace Servicios.Interfaces;

public interface IServicioAuditoria
{
    void RegistrarAltaUsuario(string email, string roles);
    void RegistrarEdicionUsuario(string email);
    void RegistrarEliminacionUsuario(string email);
    void RegistrarAltaEstadio(string nombre);
    void RegistrarEdicionEstadio(string nombre);
    void RegistrarEliminacionEstadio(string nombre);
    void RegistrarAltaEquipo(string nombre);
    void RegistrarEdicionEquipo(string nombre);
    void RegistrarEliminacionEquipo(string nombre);
    void RegistrarGeneracionAutomaticaEquipos(int cantidad, int semilla, int rankingMin, int rankingMax, string desglosePorConfederacion);
    void RegistrarGeneracionFixture();
    void RegistrarModificacionPartido(string detalle);
    void RegistrarSorteoCruces();
    void RegistrarImportacionEquipos(string mensaje, bool esExito);
  
    void RegistrarGeneracionNotificacion(string usuario,string mensaje);
    void RegistrarLecturaNotificacion(string mensajeNotificacion);

    void RegistrarSimulacion(int semilla);
    void RegistrarCalculosDeCambioDeRanking(int valorAnterior, int valorNuevo);
    
    void RegistrarExportacionArchivos(string nombreArchivo);

    List<string> ObtenerRegistrosFormateados();
}