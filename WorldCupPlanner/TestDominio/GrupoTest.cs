using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Clases;

namespace TestDominio;

[TestClass]
public class GrupoTest
{    
    [TestMethod]
    public void CrearGrupo_Valido()
    {
        Grupo grupo = new Grupo("A");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearGrupo_NombreVacio_TiroException()
    {
        Grupo grupo = new Grupo("");
    }
}