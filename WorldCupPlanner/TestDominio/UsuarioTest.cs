using System;
using Dominio.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDominio;

[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuario()
    {
        Usuario usuarioPrueba = new Usuario("Fede", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "ave123");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NuevoUsuario_SiNombreEsInvalido_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "ave123");   
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NuevoUsuario_SiApellidoEsNulo_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "", "a@gmail.com", new DateTime(2005, 05 ,20), "23456abbbbAA.");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoEmail_SiEmailEsVacio_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "Gonzalez", "", new DateTime(2005 , 05 , 20), "23456abbbbAA.");
    }
}