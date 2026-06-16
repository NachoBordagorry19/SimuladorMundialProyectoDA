using Dominio.Clases;
using Dominio.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Servicios.Interfaces;

namespace TestServicios;

[TestClass]
public class EquipoServiciosTest
{
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;
    private IEquipoRepositorio _equipoRepositorio;
    private EquipoServicios _equipoServicios;
    private EquipoDTO _equipoDTO;
    private EquipoDTO _equipoDTO2;
    private Mock<IServicioAuditoria> _auditoriaMock;

    [TestInitialize]
    public void Inicializar()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _equipoRepositorio = new EquipoRepositorioSql(_contexto);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _equipoServicios = new EquipoServicios(_equipoRepositorio, _auditoriaMock.Object);


        Confederacion confederacion = new Confederacion();
        confederacion = Confederacion.UEFA;

        _equipoDTO = new EquipoDTO()
        {
            Nombre = "Alianzz Arena",
            Confederacion = confederacion,
            RankingFifa = 2000
        };

        Confederacion confederacion2 = new Confederacion();
        confederacion2 = Confederacion.CONMEBOL;

        _equipoDTO2 = new EquipoDTO()
        {
            Nombre = "Centenario",
            Confederacion = confederacion2,
            RankingFifa = 1800
        };
    }

    [TestMethod]
    public void AgregarEquipo()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        Assert.AreEqual("Alianzz Arena", _equipoDTO.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEquipo_SiExiste_LanzaExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.AgregarEquipo(_equipoDTO);
    }

    [TestMethod]
    public void AgregarEquipo_DebeLlamarAuditoria()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _auditoriaMock.Verify(a => a.RegistrarAltaEquipo(_equipoDTO.Nombre), Times.Once);
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
        EquipoDTO equipoPrueba = _equipoServicios.ObtenerEquipo(_equipoDTO.Nombre);
        Assert.AreEqual(equipoPrueba.Nombre, _equipoDTO.Nombre);
        Assert.AreEqual(equipoPrueba.Confederacion, _equipoDTO.Confederacion);
        Assert.AreEqual(equipoPrueba.RankingFifa, _equipoDTO.RankingFifa);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerEquipo_SiNoExiste_LanzaExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.ObtenerEquipo(_equipoDTO2.Nombre);
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
    public void EliminarEquipo_DebeRegistrarEnAuditoria()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        _equipoServicios.EliminarEquipo(_equipoDTO);

        _auditoriaMock.Verify(a => a.RegistrarEliminacionEquipo(_equipoDTO.Nombre), Times.Once);
    }

    [TestMethod]
    public void ActualizarEquipo_SiExiste_SeActualiza()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        var equipoActualizadoDto = new EquipoDTO()
        {
            Nombre = _equipoDTO.Nombre,
            Confederacion = Confederacion.CONMEBOL,
            RankingFifa = 1700
        };
        _equipoServicios.ActualizarEquipo(equipoActualizadoDto);
        var equipoObtenido = _equipoServicios.ObtenerEquipo(_equipoDTO.Nombre);
        Assert.AreEqual(1700, equipoObtenido.RankingFifa);
        Assert.AreEqual(Confederacion.CONMEBOL, equipoObtenido.Confederacion);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarEquipo_SiNoExiste_LanzoExcepcion()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO2);
        var equipoActualizadoDto = new EquipoDTO()
        {
            Nombre = _equipoDTO.Nombre,
            Confederacion = Confederacion.CONMEBOL,
            RankingFifa = 1700
        };
        _equipoServicios.ActualizarEquipo(equipoActualizadoDto);
    }

    [TestMethod]
    public void ActualizarEquipo_DebeRegistrarEnAuditoria()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);
        string nombreNuevo = "Allianz Stadium";
        var dtoNuevoNombre = new EquipoDTO
        {
            Nombre = nombreNuevo,
            Confederacion = _equipoDTO.Confederacion,
            RankingFifa = _equipoDTO.RankingFifa
        };

        _equipoServicios.ActualizarEquipo(_equipoDTO.Nombre, dtoNuevoNombre);
        _auditoriaMock.Verify(a => a.RegistrarEdicionEquipo(nombreNuevo), Times.Once);

        var dtoCambioRanking = new EquipoDTO
        {
            Nombre = nombreNuevo,
            Confederacion = _equipoDTO.Confederacion,
            RankingFifa = 2500
        };

        _equipoServicios.ActualizarEquipo(dtoCambioRanking);
        _auditoriaMock.Verify(a => a.RegistrarEdicionEquipo(nombreNuevo), Times.Exactly(2));
    }

    [TestMethod]
    public void AgregarEquipo_OFC_Cupo1_PermiteUnEquipo()
    {
        var dto = new EquipoDTO()
        {
            Nombre = "OFC-Team1",
            Confederacion = Confederacion.OFC,
            RankingFifa = 1600
        };

        _equipoServicios.AgregarEquipo(dto);

        var equiposOFC = _equipoServicios.ObtenerEquipos().Where(e => e.Confederacion == Confederacion.OFC).ToList();
        Assert.AreEqual(1, equiposOFC.Count);
        Assert.AreEqual("OFC-Team1", equiposOFC[0].Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarEquipo_OFC_Cupo1_LanzaExcepcionAlExceder()
    {
        var EquipoDTOPrueba = new EquipoDTO()
        {
            Nombre = "Equipo1",
            Confederacion = Confederacion.OFC,
            RankingFifa = 1500
        };

        var Equipodto2Prueba = new EquipoDTO()
        {
            Nombre = "Equipo2",
            Confederacion = Confederacion.OFC,
            RankingFifa = 1400
        };

        _equipoServicios.AgregarEquipo(EquipoDTOPrueba);
        _equipoServicios.AgregarEquipo(Equipodto2Prueba);
    }

    [TestMethod]
    public void OrdenaYNoGeneraAuditoriaSiNoHayEmpate()
    {
        var equipos = new List<EquipoDTO>
        {
            new EquipoDTO { Nombre = "Zeta", Confederacion = Confederacion.UEFA, RankingFifa = 2000 },
            new EquipoDTO { Nombre = "Bravo", Confederacion = Confederacion.CAF, RankingFifa = 1500 }
        };

        int semilla = 123;

        var resultado = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semilla);

        var nombres = resultado.EquiposOrdenados.Select(e => e.Nombre).ToList();

        Assert.AreEqual("Zeta", nombres[0]);
        Assert.AreEqual(0, resultado.AuditoriaEmpates.Count);
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
        new EquipoDTO { Nombre = "EquipoA", Confederacion = Confederacion.UEFA, RankingFifa = 2000 },
        new EquipoDTO { Nombre = "EquipoB", Confederacion = Confederacion.UEFA, RankingFifa = 2000 },
        new EquipoDTO { Nombre = "EquipoC", Confederacion = Confederacion.CAF,  RankingFifa = 1500 }
    };
        int semillaFixture = 12345;

        var r1 = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semillaFixture);
        var r2 = _equipoServicios.ResolverEmpatesYOrdenar(equipos, semillaFixture);

        var nombres1 = r1.EquiposOrdenados.Select(e => e.Nombre).ToList();
        var nombres2 = r2.EquiposOrdenados.Select(e => e.Nombre).ToList();
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

        var nombresEntrada = equipos.Select(e => e.Nombre).OrderBy(n => n).ToList();
        var nombresSalida = r1.EquiposOrdenados.Select(e => e.Nombre).OrderBy(n => n).ToList();
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
            if (equipo.Nombre == "UEFA_01")
            {
                existeNombreFormateado = true;
            }
        }

        Assert.IsTrue(existeNombreFormateado);
    }

    [TestMethod]
    public void GenerarEquiposAutomaticamente_GeneraCantidadCorrectaPorConfederacion()
    {
        _equipoServicios.GenerarEquiposAutomaticamente(123);
        var equipos = _equipoServicios.ObtenerEquipos();

        Assert.AreEqual(16, equipos.Count(e => e.Confederacion == Confederacion.UEFA));
        Assert.AreEqual(7,  equipos.Count(e => e.Confederacion == Confederacion.CONMEBOL));
        Assert.AreEqual(7,  equipos.Count(e => e.Confederacion == Confederacion.CONCACAF));
        Assert.AreEqual(9,  equipos.Count(e => e.Confederacion == Confederacion.CAF));
        Assert.AreEqual(8,  equipos.Count(e => e.Confederacion == Confederacion.AFC));
        Assert.AreEqual(1,  equipos.Count(e => e.Confederacion == Confederacion.OFC));
    }
    
    [TestMethod]
    public void AgregarEquipo_ConBandera_GuardaLaBandera()
    {
        EquipoDTO equipoDto = new EquipoDTO
        {
            Nombre = "Argentina",
            Confederacion = Confederacion.CONMEBOL,
            RankingFifa = 1800,
            BanderaBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAUA"
        };

        _equipoServicios.AgregarEquipo(equipoDto);

        var equipoObtenido = _equipoServicios.ObtenerEquipo("Argentina");
        Assert.AreEqual("iVBORw0KGgoAAAANSUhEUgAAAAUA", equipoObtenido.BanderaBase64);
    }
    
    [TestMethod]
    public void ActualizarEquipo_ConBandera_ActualizaLaBandera()
    {
        _equipoServicios.AgregarEquipo(_equipoDTO);

        var dtoActualizado = new EquipoDTO
        {
            Nombre = _equipoDTO.Nombre,
            Confederacion = _equipoDTO.Confederacion,
            RankingFifa = _equipoDTO.RankingFifa,
            BanderaBase64 = "banderaActualizada"
        };

        _equipoServicios.ActualizarEquipo(dtoActualizado);

        var equipoObtenido = _equipoServicios.ObtenerEquipo(_equipoDTO.Nombre);
        Assert.AreEqual("banderaActualizada", equipoObtenido.BanderaBase64);
    }


}
