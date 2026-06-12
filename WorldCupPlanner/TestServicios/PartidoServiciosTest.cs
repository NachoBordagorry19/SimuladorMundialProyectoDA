using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;
using Moq;
using Servicios.Interfaces;

namespace TestServicios;

[TestClass]
public class PartidoServiciosTest
{
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;
    private IPartidoRepositorio _partidoRepositorio;
    private PartidoServicios _servicioPartido;
    private PartidoDTO _partidoDTO;
    private EstadioDTO estadioDTO;
    private EquipoDTO equipoLocalDTO;
    private EquipoDTO equipoVisitanteDTO;
    private Partido partido;
    private Mock<IServicioAuditoria> _auditoriaMock;

    [TestInitialize]
    public void Inicializar()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Equipos.RemoveRange(_contexto.Equipos);
        _contexto.SaveChanges();
        _partidoRepositorio = new PartidoRepositorioSql(_contexto);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        var equipoRepositorio = new EquipoRepositorioSql(_contexto);
        var rankingDinamico = new RankingDinamicoServicio(equipoRepositorio, _auditoriaMock.Object);
        _servicioPartido = new PartidoServicios(_partidoRepositorio, _auditoriaMock.Object, rankingDinamico);

        estadioDTO = new EstadioDTO()
        {
            Nombre = "Alianz Arena",
            Ciudad = "Alemania",
            Descripcion = "El mejor estadio del mundo",
            CapacidadLocativa = 75000
        };

        Confederacion _confederacion = new Confederacion();
        _confederacion = Confederacion.UEFA;

        equipoLocalDTO = new EquipoDTO()
        {
            nombre = "Bayern Munich",
            confederacion = _confederacion,
            rankingFifa = 2000
        };

        equipoVisitanteDTO = new EquipoDTO()
        {
            nombre = "Real Madrid",
            confederacion = _confederacion,
            rankingFifa = 1900
        };

        _partidoDTO = new PartidoDTO
        {

            Fecha = new DateTime(2026, 6, 1),
            Estadio = estadioDTO,
            equipoLocal = equipoLocalDTO,
            equipoVisitante = equipoVisitanteDTO,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Pendiente,
            golesLocal = 0,
            golesVisitante = 0
        };

    }

    [TestMethod]
    public void AgregarPartido()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        var partidos = _partidoRepositorio.ObtenerPartidos();
        Assert.AreEqual(1, partidos.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarPartido_SiPartidoEsNulo_LanzoExcepcion()
    {
        _servicioPartido.AgregarPartido(null, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
    }

    [TestMethod]
    public void ObtenerPartidos_SiHayPartidos_LosTraigo()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
        List<PartidoDTO> partidosPrueba = _servicioPartido.ObtenerPartidos();
        Assert.AreEqual(1, partidosPrueba.Count);
    }

    [TestMethod]
    public void ObtenerPartidos_SiNoHayPartidos_RetornaListaVacia()
    {
        List<PartidoDTO> partidosPrueba = _servicioPartido.ObtenerPartidos();
        Assert.AreEqual(0, partidosPrueba.Count);
    }

    [TestMethod]
    public void ObtenerPartido_SiPartidoValido_SeObtieneCorrectamente()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
        Partido partido = _partidoRepositorio.ObtenerPartidos().FirstOrDefault();
        int id = partido.Id;
        PartidoDTO partidoPrueba = _servicioPartido.ObtenerPartido(id);
        Assert.AreEqual(partidoPrueba.equipoLocal.nombre, _partidoDTO.equipoLocal.nombre);
        Assert.AreEqual(partidoPrueba.equipoVisitante.nombre, _partidoDTO.equipoVisitante.nombre);
        Assert.AreEqual(partidoPrueba.Fecha, _partidoDTO.Fecha);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerPartido_SiPartidoNoExiste_SeLanzaExcepcion()
    {
        PartidoDTO partidoDto = _servicioPartido.ObtenerPartido(-1);
    }

    [TestMethod]
    public void EliminarPartido_SiPartidoValido_SeEliminaCorrectamente()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
        _servicioPartido.EliminarPartido(_partidoDTO);
        Assert.AreEqual(0, _servicioPartido.ObtenerPartidos().Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarPartido_SiPartidoNoValido_SeLanzaExcepcion()
    {
        _servicioPartido.EliminarPartido(_partidoDTO);
    }


    [TestMethod]
    public void ActualizarPartido_SiPartidoValido_SeActualizaCorrectamente()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
        Partido partido = _partidoRepositorio.ObtenerPartidos().FirstOrDefault();
        int id = partido.Id;

        DateTime nuevaFecha = new DateTime(2026, 6, 4);
        _partidoDTO.Fecha = nuevaFecha;

        _servicioPartido.ActualizarPartido(_partidoDTO);

        PartidoDTO partidoActualizado = _servicioPartido.ObtenerPartido(id);
        Assert.AreEqual(nuevaFecha, partidoActualizado.Fecha);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarPartido_SiPartidoNoValido_LanzoExcepcion()
    {
        _servicioPartido.ActualizarPartido(_partidoDTO);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarPartido_SiPartidoNulo_LanzoExcepcion()
    {
        _servicioPartido.ActualizarPartido(null);
    }

    [TestMethod]
    public void ActualizarPartido_SiCargaResultado_SeMarcoComoJugado()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
        Partido partido = _partidoRepositorio.ObtenerPartidos().FirstOrDefault();
        int id = partido.Id;

        _partidoDTO.golesLocal = 3;
        _partidoDTO.golesVisitante = 2;
        _servicioPartido.ActualizarPartido(_partidoDTO);

        PartidoDTO partidoActualizado = _servicioPartido.ObtenerPartido(id);
        Assert.AreEqual(3, partidoActualizado.golesLocal);
        Assert.AreEqual(2, partidoActualizado.golesVisitante);
        Assert.AreEqual(EstadoPartido.Jugado, partidoActualizado.estadoPartido);
    }

    [TestMethod]
    public void ActualizarPartido_DebeRegistrarAuditoria()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _partidoDTO.golesLocal = 2;
        _servicioPartido.ActualizarPartido(_partidoDTO);

        string detalleEsperado = "Bayern Munich vs Real Madrid";
        _auditoriaMock.Verify(a => a.RegistrarModificacionPartido(detalleEsperado), Times.Once);
    }

    [TestMethod]
    public void SimularResultado_SiPartidoValido_SimulaCorrectamente()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularResultado(_partidoDTO, 12345);

        Assert.IsTrue(_partidoDTO.golesLocal >= 0);
        Assert.IsTrue(_partidoDTO.golesVisitante >= 0);
        Assert.AreEqual(EstadoPartido.Jugado, _partidoDTO.estadoPartido);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SimularResultado_SiPartidoEsNulo_LanzaExcepcion()
    {
        _servicioPartido.SimularResultado(null, 12345);
    }



    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SimularResultado_SiPartidoNoExiste_LanzaExcepcion()
    {
        _partidoDTO.idPartido = -1;
        _servicioPartido.SimularResultado(_partidoDTO, 12345);
    }
    
    [TestMethod]
    public void AgregarPartido_SiEstadoEsJugado_SeGuardaJugado()
    {
        _partidoDTO.estadoPartido = EstadoPartido.Jugado;

        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        PartidoDTO partidoGuardado = _servicioPartido.ObtenerPartido(_partidoDTO.idPartido);
        Assert.AreEqual(EstadoPartido.Jugado, partidoGuardado.estadoPartido);
    }
    
    [TestMethod]
    public void ObtenerPartidosFiltrados_SiCoincideConFiltros_RetornaUnPartido()
    {
        _partidoDTO.Grupo = "A";
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        List<PartidoDTO> partidos = _servicioPartido.ObtenerPartidosFiltrados(
            _partidoDTO.Fecha,
            estadioDTO.Nombre,
            "A",
            Fase.Grupos.ToString()
        );

        Assert.AreEqual(1, partidos.Count);
    }
    
    [TestMethod]
    public void ObtenerEstadiosGruposYFases_DePartidos_RetornaDatos()
    {
        _partidoDTO.Grupo = "A";
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        Assert.AreEqual(1, _servicioPartido.ObtenerEstadiosDePartidos().Count);
        Assert.AreEqual(1, _servicioPartido.ObtenerGruposDePartidos().Count);
        Assert.AreEqual(1, _servicioPartido.ObtenerFasesDePartidos().Count);
    }
    
    [TestMethod]
    public void ActualizarPartido_SiCambiaEstadio_SeActualiza()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _partidoDTO.Estadio = new EstadioDTO
        {
            Nombre = "Nuevo Estadio",
            Ciudad = "Montevideo",
            Descripcion = "Nuevo",
            CapacidadLocativa = 60000
        };

        _servicioPartido.ActualizarPartido(_partidoDTO);

        PartidoDTO partidoActualizado = _servicioPartido.ObtenerPartido(_partidoDTO.idPartido);
        Assert.AreEqual("Nuevo Estadio", partidoActualizado.Estadio.Nombre);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarPartido_SiFaseEstaBloqueada_LanzaExcepcion()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.BloquearEdicionFase(Fase.Grupos);

        _partidoDTO.Fecha = new DateTime(2026, 6, 10);
        _servicioPartido.ActualizarPartido(_partidoDTO);
    }
    
    [TestMethod]
    public void SimularResultado_SiEsEliminatorio_NoQuedaEmpatado()
    {
        _partidoDTO.fase = Fase.Dieciseisavos;
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularResultado(_partidoDTO, 12345);

        Assert.AreNotEqual(_partidoDTO.golesLocal, _partidoDTO.golesVisitante);
    }
    
    [TestMethod]
    public void SimularTodosLosPartidos_SiHayPendiente_LoMarcaComoJugado()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularTodosLosPartidos(12345);

        PartidoDTO partidoSimulado = _servicioPartido.ObtenerPartido(_partidoDTO.idPartido);
        Assert.AreEqual(EstadoPartido.Jugado, partidoSimulado.estadoPartido);
    }

    [TestMethod]
    public void AgregarPartido_SeAgregaPartidoConIncidenciaLocal()
    {
        _partidoDTO.incidenciaEquipoLocal = new List<TipoIncidencia>();
        _partidoDTO.incidenciaEquipoLocal.Add(TipoIncidencia.TarjetaAmarilla);
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
    }

    [TestMethod]
    public void AgregarPartido_SeAgregaPartidoConIncidenciaVisitante()
    {
        _partidoDTO.incidenciaEquipoVisitante = new List<TipoIncidencia>();
        _partidoDTO.incidenciaEquipoVisitante.Add(TipoIncidencia.TarjetaAmarilla);
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);
    }

    [TestMethod]
    public void SimularResultado_GeneraIncidenciasParaAmbosEquipos()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularResultado(_partidoDTO, 12345);

        PartidoDTO partidoSimulado = _servicioPartido.ObtenerPartido(_partidoDTO.idPartido);
        Assert.IsTrue(partidoSimulado.incidenciaEquipoLocal.Count >= 0 && partidoSimulado.incidenciaEquipoLocal.Count <= 10);
        Assert.IsTrue(partidoSimulado.incidenciaEquipoVisitante.Count >= 0 && partidoSimulado.incidenciaEquipoVisitante.Count <= 10);
    }

    [TestMethod]
    public void SimularResultado_GeneraTarjetasRojasEnRangoValido()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularResultado(_partidoDTO, 12345);

        PartidoDTO partidoSimulado = _servicioPartido.ObtenerPartido(_partidoDTO.idPartido);
        int rojasLocal = partidoSimulado.incidenciaEquipoLocal.Count(i => i == TipoIncidencia.TarjetaRoja);
        int rojasVisitante = partidoSimulado.incidenciaEquipoVisitante.Count(i => i == TipoIncidencia.TarjetaRoja);
        Assert.IsTrue(rojasLocal >= 0 && rojasLocal <= 5);
        Assert.IsTrue(rojasVisitante >= 0 && rojasVisitante <= 5);
    }

    [TestMethod]
    public void SimularResultado_ConMotor_PartidoQuedaJugado()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO, equipoVisitanteDTO, estadioDTO);

        _servicioPartido.SimularResultado(_partidoDTO, 12345, new MotorAleatorioPuro());

        Assert.AreEqual(EstadoPartido.Jugado, _partidoDTO.estadoPartido);
    }
}