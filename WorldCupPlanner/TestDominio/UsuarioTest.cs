using System;
using Dominio.Clases;
using Dominio.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestDominio;

[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuario()
    {
        Usuario usuarioPrueba = new Usuario("Fede", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "aveAA@e123", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NuevoUsuario_SiNombreEsInvalido_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "ave123", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NuevoUsuario_SiApellidoEsNulo_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "", "a@gmail.com", new DateTime(2005, 05, 20), "23456abbbbAA.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoEmail_SiEmailEsVacio_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "", new DateTime(2005, 05, 20), "23456abbbbAA.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoFecha_SiFechaVieneVacia_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", DateTime.MinValue, "23456abbbbAA.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaEsVacia_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 03, 20), "", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneMayuscula_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 05, 20), "asdfg23455.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneMinuscula_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 05, 20), "ASDFGDS2344.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneNumero_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 05, 20), "AASDSAasddsa.", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraseñaNoTieneCaracterEspecial_TiroExcepcion()
    {
        Usuario usuarioPrueba = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 05, 20), "ADDSSSDSAsdsa23", Rol.Administrador);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void InsertoContraseña_SiContraTieneMenosDe8Caracteres_TiroExcepcion()
    {
        Usuario usuario = new Usuario("Federico", "Gonzalez", "a@gmail.com", new DateTime(2005, 05, 20), "av34.A", Rol.Administrador);
    }

    [TestMethod]
    public void CrearUsuario_PermiteRolesNoExcluyentes()
    {
        Usuario usuarioPrueba = new Usuario("Fede", "Gonzales", "a@gmail.com", new DateTime(2003, 08, 23), "aveAA@e123", new List<Rol> { Rol.Administrador, Rol.Editor });

        Assert.IsTrue(usuarioPrueba.Roles.Contains(Rol.Administrador));
        Assert.IsTrue(usuarioPrueba.Roles.Contains(Rol.Editor));
    }

}