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
        _equipoRepositorio = new EquipoRepositorio(_baseDeDatosEnMemoria);
        _equipoServicios = new EquipoServicios(_equipoRepositorio);

        Confederacion confederacion = new Confederacion();
        confederacion = Confederacion.UEFA;

        _equipoDTO = new EquipoDTO()
        {
            nombre = "Alianzz Arena",
            confederacion = confederacion,
            rankingFifa = 1
        };

        Confederacion confederacion2 = new Confederacion();
        confederacion2 = Confederacion.CONMEBOL;

        _equipoDTO2 = new EquipoDTO()
        {
            nombre = "Centenario",
            confederacion = confederacion2,
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
        Assert.AreEqual(2, equipos.Count);
    }

    [TestMethod]
    public void ObtenerEquipo_SiExiste_DevuelveEquipo()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        EquipoDTO equipoPrueba = _equipoServicios.ObtenerEquipo(_equipoDTO.nombre);
        Assert.AreEqual(equipoPrueba.nombre, _equipoDTO.nombre);
        Assert.AreEqual(equipoPrueba.confederacion, _equipoDTO.confederacion);
        Assert.AreEqual(equipoPrueba.rankingFifa, _equipoDTO.rankingFifa);
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
        Assert.AreEqual(1, equiposDtosIniciales.Count);
        _equipoServicios.EliminarEquipo(_equipoDTO);
        List<EquipoDTO> equiposDtos = _equipoServicios.ObtenerEquipos();
        Assert.AreEqual(0, equiposDtos.Count);
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
    public void OrdenaYNoGeneraAuditoriaSiNoHayEmpate()
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
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RecibeNingunEquipo_LanzaExcepcion()
    {
        var equipos = new List<EquipoDTO>
        {
            null,
            null
        };
        int semilla = 124;
        var resultado = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semilla);
    }

    [TestMethod]
    public void ResolverEmpates_MismaSemilla_OrdenYAuditoriaDeterministica()
    {
        var equipos = new List<EquipoDTO>
    {
        new EquipoDTO { nombre = "EquipoA", confederacion = Confederacion.UEFA, rankingFifa = 10 },
        new EquipoDTO { nombre = "EquipoB", confederacion = Confederacion.UEFA, rankingFifa = 10 },
        new EquipoDTO { nombre = "EquipoC", confederacion = Confederacion.CAF,  rankingFifa = 5 }
    };
        int semillaFixture = 12345;

        var r1 = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semillaFixture);
        var r2 = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semillaFixture);

        var nombres1 = r1.EquiposOrdenados.Select(e => e.nombre).ToList();
        var nombres2 = r2.EquiposOrdenados.Select(e => e.nombre).ToList();
        CollectionAssert.AreEqual(nombres1, nombres2);

        Assert.AreEqual(r1.AuditoriaEmpates.Count, r2.AuditoriaEmpates.Count);

        for (int i = 0; i < r1.AuditoriaEmpates.Count; i++)
        {
            var a1 = r1.AuditoriaEmpates[i];
            var a2 = r2.AuditoriaEmpates[i];

            Assert.AreEqual(a1.RankingFifa, a2.RankingFifa);
            Assert.AreEqual(a1.SemillaUsada, a2.SemillaUsada);
            CollectionAssert.AreEqual(a1.OrdenResuelto, a2.OrdenResuelto);
        }

        var nombresEntrada = equipos.Select(e => e.nombre).OrderBy(n => n).ToList();
        var nombresSalida = r1.EquiposOrdenados.Select(e => e.nombre).OrderBy(n => n).ToList();
        CollectionAssert.AreEqual(nombresEntrada, nombresSalida);
    }

    [TestMethod]
    public void GenerarEquiposAutomaticamente_DebeCompletar48Equipos_EnTotal()
    {
        int semillaCompletar = 123;
        _equipoServicios.GenerarEquiposAutomaticamente(semillaCompletar);
        var equipos = _equipoServicios.ObtenerEquipos();
        Assert.AreEqual(48, equipos.Count);
    }
    
    [TestMethod]
    public void GenerarEquiposAutomaticamente_DebeGenerarNombresConFormatoCorrecto()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        List<EquipoDTO> listaEquipos = _equipoServicios.ObtenerEquipos();

        bool existeNombreFormateado = false;
        foreach (EquipoDTO equipo in listaEquipos)
        {
            if (equipo.nombre == "UEFA_01")
            {
                existeNombreFormateado = true;
            }
        }

        Assert.IsTrue(existeNombreFormateado);
    }
    
    [TestMethod]
    public void GenerarEquiposAutomaticamente_DebeRegistrarAuditoria()
    {
        int semilla = 666;
        List<string> auditoria = _equipoServicios.GenerarEquiposAutomaticamente(semilla);

        Assert.IsNotNull(auditoria);
        Assert.IsTrue(auditoria.Count > 0);
    
        bool registroCorrecto = false;
        foreach (string log in auditoria)
        {
            if (log.Contains("666"))
            {
                registroCorrecto = true;
            }
        }
        Assert.IsTrue(registroCorrecto);
    }
    
    [TestMethod]
    public void GenerarEquiposAutomaticamente_SiYaEstaLleno_InformaEnAuditoria()
    {
        int semilla = 666;
        _equipoServicios.GenerarEquiposAutomaticamente(semilla);
        List<string> resultado = _equipoServicios.GenerarEquiposAutomaticamente(semilla);

        bool mensajeEncontrado = false;
        foreach (string linea in resultado)
        {
            if (linea.Contains("El cupo total de 48 equipos ya está completo."))
            {
                mensajeEncontrado = true;
            }
        }

        Assert.IsTrue(mensajeEncontrado);
    }
}
