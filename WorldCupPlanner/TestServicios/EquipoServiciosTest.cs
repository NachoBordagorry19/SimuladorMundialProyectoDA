using Dominio.Clases;
using Dominio.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;
using System.Collections.Generic;
using System.Linq;

namespace TestServicios;

[TestClass]
public class EquipoServiciosTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IEquipoRepositorio _equipoRepositorio;
    private EquipoServicios _equipoServicios;
    private EquipoDTO _equipoDTO;
    private EquipoDTO _equipoDTO2;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _equipoRepositorio = new  EquipoRepositorio(_baseDeDatosEnMemoria);
        _equipoServicios = new EquipoServicios(_equipoRepositorio);
        
        Confederacion _confederacion = new Confederacion();
        _confederacion = Confederacion.UEFA;
        
        _equipoDTO = new EquipoDTO()
        {
            nombre = "Alianzz Arena",
            confederacion = _confederacion,
            rankingFifa = 1
        };

        Confederacion _confederacion2 = new Confederacion();
        _confederacion2 = Confederacion.CONMEBOL;
        
        _equipoDTO2 = new EquipoDTO()
        {
            nombre = "Centenario",
            confederacion = _confederacion2,
            rankingFifa = 23
        };
    }
    
    [TestMethod]
    public void AgregarEquipo()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        Assert.AreEqual("Alianzz Arena", _equipoDTO.nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEquipo_SiExiste_LanzaExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.AgregarEquipo(_equipoDTO);
    }

    [TestMethod]
    public void ObtenerEquipos_DevuelveTodosLosEquipo()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.AgregarEquipo(_equipoDTO2);
        List<EquipoDTO> equipos = _equipoServicios.ObtenerEquipos();
        Assert.AreEqual(2,equipos.Count);
    }

    [TestMethod]
    public void ObtenerEquipo_SiExiste_DevuelveEquipo()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        EquipoDTO equipoPrueba = _equipoServicios.ObtenerEquipo(_equipoDTO.nombre);
        Assert.AreEqual(equipoPrueba.nombre, _equipoDTO.nombre);
        Assert.AreEqual(equipoPrueba.confederacion,_equipoDTO.confederacion);
        Assert.AreEqual(equipoPrueba.rankingFifa,_equipoDTO.rankingFifa);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerEquipo_SiNoExiste_LanzaExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.ObtenerEquipo(_equipoDTO2.nombre);
    }

    [TestMethod]
    public void EliminarEquipo_SiExiste()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        List<EquipoDTO> equiposDtosIniciales = _equipoServicios.ObtenerEquipos();
        Assert.AreEqual(1,equiposDtosIniciales.Count);
        _equipoServicios.EliminarEquipo(_equipoDTO);
        List<EquipoDTO> equiposDtos = _equipoServicios.ObtenerEquipos();
        Assert.AreEqual(0,equiposDtos.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarEquipo_SiNoExiste_LanzoExcepcio()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.EliminarEquipo(_equipoDTO2);
    }

    [TestMethod]
    public void ActualizarEquipo_SiExiste_SeActualiza()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        var equipoActualizadoDto = new EquipoDTO()
        {
            nombre = _equipoDTO.nombre, 
            confederacion = Confederacion.CONMEBOL,
            rankingFifa = 99
        };
        _equipoServicios.ActualizarEquipo(equipoActualizadoDto);
        var equipoObtenido = _equipoServicios.ObtenerEquipo(_equipoDTO.nombre);
        Assert.AreEqual(99, equipoObtenido.rankingFifa);
        Assert.AreEqual(Confederacion.CONMEBOL, equipoObtenido.confederacion);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarEquipo_SiNoExiste_LanzoExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO2);
        var equipoActualizadoDto = new EquipoDTO()
        {
            nombre = _equipoDTO.nombre, 
            confederacion = Confederacion.CONMEBOL,
            rankingFifa = 99
        };
        _equipoServicios.ActualizarEquipo(equipoActualizadoDto);
    }

    [TestMethod]
    public void AgregarEquipo_OFC_Cupo1_PermiteUnEquipo()
    {
        var dto = new EquipoDTO()
        {
            nombre = "OFC-Team1",
            confederacion = Confederacion.OFC,
            rankingFifa = 50
        };

        _equipoServicios.AgregarEquipo(dto);

        var equiposOFC = _equipoServicios.ObtenerEquipos().Where(e => e.confederacion == Confederacion.OFC).ToList();
        Assert.AreEqual(1, equiposOFC.Count);
        Assert.AreEqual("OFC-Team1", equiposOFC[0].nombre);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEquipo_OFC_Cupo1_LanzaExcepcionAlExceder()
    {
        var EquipoDTOPrueba = new EquipoDTO()
        {
            nombre = "Equipo1",
            confederacion = Confederacion.OFC,
            rankingFifa = 45
        };

        var Equipodto2Prueba = new EquipoDTO()
        {
            nombre = "Equipo2",
            confederacion = Confederacion.OFC,
            rankingFifa = 46
        };

        _equipoServicios.AgregarEquipo(EquipoDTOPrueba);
        _equipoServicios.AgregarEquipo(Equipodto2Prueba);
    }
    
    [TestMethod]
    public void ResolverEmpates_Minimo_OrdenaYGeneraAuditoriaSiHayEmpate()
    {
        var equipos = new List<EquipoDTO>
        {
            new EquipoDTO { nombre = "Zeta", confederacion = Confederacion.UEFA, rankingFifa = 10 },
            new EquipoDTO { nombre = "Bravo", confederacion = Confederacion.CAF, rankingFifa = 5 }
        };

        int semilla = 123;
        
        var resultado = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semilla);
        
        var nombres = resultado.EquiposOrdenados.Select(e => e.nombre).ToList();
        
        Assert.AreEqual("Bravo", nombres[0]);
        
        CollectionAssert.AreEquivalent(new List<string> { "Alpha", "Zeta" }, nombres.Skip(1).ToList());
    }
}
