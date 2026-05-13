using Dominio.Clases;
using Dominio.Enums;
using Repositorio.Interfaces;

namespace Repositorio;

public class ImportadorEquiposCSV : IImportadorEquiposCSV
{
   private const string ENCABEZADO_NOMBRE = "Nombre";
   private const string ENCABEZADO_CONFEDERACION = "Confederación";
   private const string ENCABEZADO_RANKING_FIFA = "RankingFIFA";
  
   private const int CANTIDAD_COLUMNAS_ESPERADAS = 3;
  
   public List<Equipo> ImportarDesdeCSV(string rutaArchivo)
   {
      
           using (StreamReader lectorArchivo = new StreamReader(rutaArchivo))
           {
             
               string lineaEncabezados = lectorArchivo.ReadLine();


    
               Dictionary<string, int> indicesColumnas = ObtenerIndicesColumnas(lineaEncabezados);
              
               List<Equipo> equiposImportados = new List<Equipo>();
              
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
                       Equipo equipoImportado = ConvertirLineaAEquipo(linea, indicesColumnas);
                       equiposImportados.Add(equipoImportado);
                   }
                   catch (Exception excepcion)
                   {
                      
                   }


                   numeroLinea++;
               }


               return equiposImportados;
           }
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
  
   private Equipo ConvertirLineaAEquipo(string linea, Dictionary<string, int> indicesColumnas)
   {
       string[] campos = linea.Split(',');
      
      
       string nombre = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_NOMBRE);
       string confederacionTexto = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_CONFEDERACION);
       string rankingFifaTexto = ExtraerYValidarCampo(campos, indicesColumnas, ENCABEZADO_RANKING_FIFA);
      
       Confederacion confederacion = ConvertirTextoAConfederacion(confederacionTexto);
      
       int rankingFifa = ConvertirTextoAEntero(rankingFifaTexto, ENCABEZADO_RANKING_FIFA);
      
       return new Equipo(nombre, confederacion, rankingFifa);
   }
  
   private string ExtraerYValidarCampo(string[] campos, Dictionary<string, int> indicesColumnas, string nombreCampo)
   {
       int indice = indicesColumnas[nombreCampo];
       string valor = campos[indice].Trim();


       if (string.IsNullOrWhiteSpace(valor))
       {
           throw new ArgumentException($"El campo '{nombreCampo}' no puede estar vacío");
       }


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
