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
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoFecha_SiFechaVieneVacia_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "Gonzalez", "a@gmail.com", DateTime.MinValue, "23456abbbbAA.");
    }

    [TestMethod] [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaEsVacia_TiroExcepcion()
    {
        Usuario usuario = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005 , 03 , 20), "");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneMayuscula_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005 , 05 , 20), "asdfg23455.");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneMinuscula_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005 , 05 , 20), "ASDFGDS2344.");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneNumero_TiroExcepcion()
    {
        Usuario user = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005 , 05 , 20), "AASDSAasddsa.");
    }
}