using Dominio.Clases;
using Dominio.Enums;
using Moq;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;


namespace TestRepositorio;

[TestClass]
public class ImportadorEquiposCSVTest
{

    private IImportadorEquiposCSV _importadorEquipos;
    private Mock<IServicioAuditoria> _mockAuditoria;
    
    [TestInitialize]
    public void Inicializar()
    {
        _mockAuditoria = new Mock<IServicioAuditoria>();
        _importadorEquipos = new ImportadorEquiposCSV(_mockAuditoria.Object);
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

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_SinEncabezados_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal("");


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConDosColumnasEnEncabezado_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación\n" +
            "Argentina,CONMEBOL"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConCuatroColumnasEnEncabezado_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación,RankingFIFA,Puntos\n" +
            "Argentina,CONMEBOL,1500,0"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConEncabezadoIncorrecto_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Name,Confederation,FIFA\n" +
            "Argentina,CONMEBOL,1500"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }

    [TestMethod]
    public void ImportarCSV_ConColumnasEnOrdenDiferente_CreaEquipoCorrectamente()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "RankingFIFA,Nombre,Confederación\n" +
            "1500,Argentina,CONMEBOL"
        );


        try
        {
            List<Equipo> equiposImportados = _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);


            Assert.AreEqual(1, equiposImportados.Count);
            Assert.AreEqual("Argentina", equiposImportados[0].Nombre);
            Assert.AreEqual(Confederacion.CONMEBOL, equiposImportados[0].Confederacion);
            Assert.AreEqual(1500, equiposImportados[0].RankingFifa);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConNombreVacio_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación,RankingFIFA\n" +
            ",CONMEBOL,1500"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConConfederacionVacia_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación,RankingFIFA\n" +
            "Argentina,,1500"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ImportarCSV_ConRankingFifaVacio_LanzaExcepcion()
    {
        string rutaArchivoCSV = CrearArchivoCSVTemporal(
            "Nombre,Confederación,RankingFIFA\n" +
            "Argentina,CONMEBOL,"
        );


        try
        {
            _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
        }
        finally
        {
            LimpiarArchivoCSV(rutaArchivoCSV);
        }
    }
    
     [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConConfederacionInvalida_LanzaExcepcion()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,INVALID,1500"
       );


       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }


   [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConRankingFifaNoNumerico_LanzaExcepcion()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,CONMEBOL,abc"
       );


       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }


   [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConRankingFifaMenorAlMinimo_LanzaExcepcion()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,CONMEBOL,250"
       );


       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }


   [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConRankingFifaMayorAlMaximo_LanzaExcepcion()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,CONMEBOL,3000"
       );


       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }


   [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConNombreMayorA60Caracteres_LanzaExcepcion()
   {
       string nombreLargo = "Este nombre tiene mas de sesenta caracteres y debe ser rechazado!!!";
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           $"Nombre,Confederación,RankingFIFA\n" +
           $"{nombreLargo},CONMEBOL,1500"
       );


       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }

   [TestMethod]
   [ExpectedException(typeof(ArgumentException))]
   public void ImportarCSV_ConNombreDuplicado_LanzaExcepcion()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,CONMEBOL,1500\n" +
           "Argentina,UEFA,1600"
       );

       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }

   [TestMethod]
   public void ImportarCSV_ConNombresUnicos_CreaVariosEquiposCorrectamente()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Argentina,CONMEBOL,1500\n" +
           "Brasil,CONMEBOL,1600\n" +
           "Alemania,UEFA,1800"
       );

       try
       {
           List<Equipo> equiposImportados = _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);

           Assert.AreEqual(3, equiposImportados.Count);
           Assert.AreEqual("Argentina", equiposImportados[0].Nombre);
           Assert.AreEqual("Brasil", equiposImportados[1].Nombre);
           Assert.AreEqual("Alemania", equiposImportados[2].Nombre);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
   }
   
   [TestMethod]
   public void ImportarCSV_Exitoso_DeberiaLlamarAlServicioDeAuditoria()
   {
       string rutaArchivoCSV = CrearArchivoCSVTemporal(
           "Nombre,Confederación,RankingFIFA\n" +
           "Uruguay,CONMEBOL,1800"
       );
       try
       {
           _importadorEquipos.ImportarDesdeCSV(rutaArchivoCSV);
           _mockAuditoria.Verify(a => a.RegistrarImportacionEquipos(
               It.Is<string>(s => s.Contains("Importación")), 
               true
           ), Times.Once);
       }
       finally
       {
           LimpiarArchivoCSV(rutaArchivoCSV);
       }
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
