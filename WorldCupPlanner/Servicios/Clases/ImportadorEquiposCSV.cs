using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;
using Servicios.Interfaces;

namespace Repositorio;

public class ImportadorEquiposCSV : IImportadorEquiposCSV 
{
   private const string ENCABEZADO_NOMBRE = "Nombre";
   private const string ENCABEZADO_CONFEDERACION = "Confederación";
   private const string ENCABEZADO_RANKING_FIFA = "RankingFIFA";
  
   private const int CANTIDAD_COLUMNAS_ESPERADAS = 3;
   private const int NOMBRE_MAXIMO_CARACTERES = 60;
   private const int NOMBRE_MINIMO_CARACTERES = 1;
   private const int RANKING_FIFA_MINIMO = 300;
   private const int RANKING_FIFA_MAXIMO = 2500;
   private readonly IServicioAuditoria _auditoria;
   
   public ImportadorEquiposCSV(IServicioAuditoria auditoria)
   {
       _auditoria = auditoria;
   }
   public List<Equipo> ImportarDesdeCSV(string rutaArchivo) 
   {
       ValidarRutaArchivo(rutaArchivo);
       try
       {
           using (StreamReader lectorArchivo = new StreamReader(rutaArchivo))
           {
               string lineaEncabezados = lectorArchivo.ReadLine();
               ValidarEncabezados(lineaEncabezados);

               Dictionary<string, int> indicesColumnas = ObtenerIndicesColumnas(lineaEncabezados);
              
               List<Equipo> equiposImportados = new List<Equipo>();
               HashSet<string> nombresVistos = new HashSet<string>();
              
               string linea;
               int numeroLinea = 2;

               while ((linea = lectorArchivo.ReadLine()) != null)
               {
                   if (string.IsNullOrWhiteSpace(linea))
                   {
                       numeroLinea++;
                       continue;
                   }

                   try
                   {
                       Equipo equipoImportado = ConvertirLineaAEquipo(linea, indicesColumnas, nombresVistos);
                       equiposImportados.Add(equipoImportado);
                   }
                   catch (Exception excepcion)
                   {
                       throw new ArgumentException(
                           $"Error al importar equipo en línea {numeroLinea}: {excepcion.Message}",
                           excepcion
                       );
                   }

                   numeroLinea++;
               }
               return equiposImportados;
           }
       }
       catch (FileNotFoundException)
       {
           throw new ArgumentException($"El archivo CSV no existe en la ruta: {rutaArchivo}");
       }
       catch (Exception excepcion) when (!(excepcion is ArgumentException))
       {
           throw new ArgumentException(
               "Ocurrió un error inesperado al leer el archivo CSV",
               excepcion
           );
       }
   }
   
   private void ValidarRutaArchivo(string rutaArchivo)
   {
       if (string.IsNullOrWhiteSpace(rutaArchivo))
       {
           throw new ArgumentException("La ruta del archivo CSV no puede ser nula");
       }
   }
   
   private void ValidarEncabezados(string lineaEncabezados)
   {

       if (string.IsNullOrWhiteSpace(lineaEncabezados))
       {
           throw new ArgumentException("El archivo CSV debe contener una fila de encabezados");
       }
       
       string[] encabezados = lineaEncabezados.Split(',');
       
       if (encabezados.Length != CANTIDAD_COLUMNAS_ESPERADAS)
       {
          throw new ArgumentException(
              $"El CSV debe tener exactamente {CANTIDAD_COLUMNAS_ESPERADAS} columnas"
          );
       }
       
       string[] encabezadosTrimmed = encabezados.Select(e => e.Trim()).ToArray();
       
       if (!ContieneTodosLosEncabezados(encabezadosTrimmed))
       {
           throw new ArgumentException(
               $"El CSV debe contener los encabezados: {ENCABEZADO_NOMBRE}, " +
               $"{ENCABEZADO_CONFEDERACION}, {ENCABEZADO_RANKING_FIFA}"
           );
       }
   }
   private bool ContieneTodosLosEncabezados(string[] encabezados)
   {
       return encabezados.Contains(ENCABEZADO_NOMBRE) &&
              encabezados.Contains(ENCABEZADO_CONFEDERACION) &&
              encabezados.Contains(ENCABEZADO_RANKING_FIFA);
   }
  
   private Dictionary<string, int> ObtenerIndicesColumnas(string lineaEncabezados)
   {
       string[] encabezados = lineaEncabezados.Split(',');
       Dictionary<string, int> indices = new Dictionary<string, int>();
      
       for (int i = 0; i < encabezados.Length; i++)
       {
           string encabezado = encabezados[i].Trim();
           indices[encabezado] = i;
       }


       return indices;
   }
  
   private Equipo ConvertirLineaAEquipo(string linea, Dictionary<string, int> indicesColumnas, HashSet<string> nombresVistos)
   {
       string[] campos = linea.Split(',');
      
       string nombre = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_NOMBRE);
       ValidarNombre(nombre, nombresVistos);
       
       string confederacionTexto = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_CONFEDERACION);
       ValidarConfederacion(confederacionTexto);
       Confederacion confederacion = ConvertirTextoAConfederacion(confederacionTexto);
       
       string rankingFifaTexto = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_RANKING_FIFA);
       ValidarRankingFifaTexto(rankingFifaTexto);
       int rankingFifa = ConvertirTextoAEntero(rankingFifaTexto, ENCABEZADO_RANKING_FIFA);
       ValidarRankingFifaRango(rankingFifa);
      
       return new Equipo(nombre, confederacion, rankingFifa);
   }
   
   private void ValidarNombre(string nombre, HashSet<string> nombresVistos)
   {
       ValidarNombreNoVacio(nombre);
       ValidarNombreLongitud(nombre);
       ValidarNombreUnico(nombre, nombresVistos);
   }
 
   private void ValidarNombreNoVacio(string nombre)
   {
       if (string.IsNullOrWhiteSpace(nombre))
       {
           throw new ArgumentException("El nombre del equipo no puede estar vacío");
       }
   }
   private void ValidarNombreLongitud(string nombre)
   {
       if (nombre.Length < NOMBRE_MINIMO_CARACTERES)
       {
           throw new ArgumentException($"El nombre debe tener al menos {NOMBRE_MINIMO_CARACTERES} carácter");
       }
       
       if (nombre.Length > NOMBRE_MAXIMO_CARACTERES)
       {
           throw new ArgumentException($"El nombre no puede superar los {NOMBRE_MAXIMO_CARACTERES} caracteres");
       }
   }
   
   private void ValidarNombreUnico(string nombre, HashSet<string> nombresVistos)
   {
       if (!nombresVistos.Add(nombre))
       {
           throw new ArgumentException($"El nombre del equipo debe ser único, el nombre '{nombre}' ya fue visto");
       }
   }
   
   private void ValidarConfederacion(string confederacionTexto)
   {
       if (string.IsNullOrWhiteSpace(confederacionTexto))
       {
           throw new ArgumentException("La confederación no puede estar vacía");
       }
   }
   
   private void ValidarRankingFifaTexto(string rankingFifaTexto)
   {
       if (string.IsNullOrWhiteSpace(rankingFifaTexto))
       {
           throw new ArgumentException("El ranking FIFA no puede estar vacío");
       }
   }
   
   private void ValidarRankingFifaRango(int rankingFifa)
   {
       if (rankingFifa < RANKING_FIFA_MINIMO || rankingFifa > RANKING_FIFA_MAXIMO)
       {
           throw new ArgumentException(
               $"El ranking FIFA debe estar entre {RANKING_FIFA_MINIMO} y {RANKING_FIFA_MAXIMO}"
           );
       }
   }
  
   private string ExtraerYValidarCampo(string[] campos, Dictionary<string, int> indicesColumnas, string nombreCampo)
   {
       int indice = indicesColumnas[nombreCampo];
       string valor = campos[indice].Trim();
       return valor;
   }
  
   private Confederacion ConvertirTextoAConfederacion(string confederacionTexto)
   {
      
       return (Confederacion)Enum.Parse(typeof(Confederacion), confederacionTexto, ignoreCase: true);
      
   }
  
   private int ConvertirTextoAEntero(string texto, string nombreCampo)
   {
       if (!int.TryParse(texto, out int valor))
       {
           throw new ArgumentException(
               $"El campo '{nombreCampo}' debe ser un número entero válido, recibido: '{texto}'"
           );
       }


       return valor;
   }
}
