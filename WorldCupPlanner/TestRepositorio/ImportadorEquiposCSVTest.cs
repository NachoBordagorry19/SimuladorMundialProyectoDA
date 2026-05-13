using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;

namespace TestRepositorio;

[TestClass]
public class ImportadorEquiposCSVTest
{

    private IImportadorEquiposCSV _importadorEquipos;
    
    [TestInitialize]
    public void Inicializar()
    {
        _importadorEquipos = new ImportadorEquiposCSV();
    }
    
    [TestMethod]
    public void ImportarCSV_ConEquipoValido_CreaTequipoCorrectamente()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación,RankingFIFA\n" +
            "Argentina,CONMEBOL,1500"
        );

        try
        {
            List<Equipo> equiposImportados = _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
            
            Assert.AreEqual(1, equiposImportados.Count, "Debe importarse exactamente un equipo");
            
            Equipo equipoImportado = equiposImportados[0];
            Assert.AreEqual("Argentina", equipoImportado.Nombre, "El nombre debe coincidir exactamente");
            Assert.AreEqual(Confederacion.CONMEBOL, equipoImportado.Confederacion, "La confederación debe ser CONMEBOL");
            Assert.AreEqual(1500, equipoImportado.RankingFifa, "El ranking FIFA debe ser 1500");
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConRutaNula_LanzaExcepcion()
    {
        _importadorEquipos.ImportarDesdeCSV(null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConRutaVacia_LanzaExcepcion()
    {
        _importadorEquipos.ImportarDesdeCSV("");
    }
  
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConRutaSoloEspacios_LanzaExcepcion()
    {
        _importadorEquipos.ImportarDesdeCSV("   ");
    }


    
    private string CrearArchivoCSVTemporal(string contenido)
    {
        string rutaArchivo = Path.Combine(Path.GetTempPath(), $"equipos_test_{Guid.NewGuid()}.csv");
        File.WriteAllText(rutaArchivo, contenido);
        return rutaArchivo;
    }

    private void LimpiarArchivoCSV(string rutaArchivo)
    {
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
        }
    }
}
