using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;
using Dominio.Clases;
using Dominio.Enums;

namespace TestServicios;

[TestClass]
public class PartidoServiciosTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IPartidoRepositorio _partidoRepositorio;
    private PartidoServicios _servicioPartido;
    private PartidoDTO _partidoDTO;
    private EstadioDTO estadioDTO;
    private EquipoDTO equipoLocalDTO;
    private EquipoDTO equipoVisitanteDTO;
    private Partido partido;
    
    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatosEnMemoria);
        _servicioPartido = new PartidoServicios(_partidoRepositorio);
        
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
            rankingFifa = 2
        };
        
        equipoVisitanteDTO = new EquipoDTO()
        {
            nombre = "Real Madrid",
            confederacion = _confederacion,
            rankingFifa = 3
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
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO,equipoVisitanteDTO, estadioDTO);

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
        _servicioPartido.AgregarPartido(_partidoDTO,equipoLocalDTO,equipoVisitanteDTO, estadioDTO);
        List<PartidoDTO> partidosPrueba = _servicioPartido.ObtenerPartidos();
        Assert.AreEqual(1, partidosPrueba.Count);
    }
    
    [TestMethod]
    public void ObtenerPartido_SiPartidoValido_SeObtieneCorrectamente()
    {
        _servicioPartido.AgregarPartido(_partidoDTO, equipoLocalDTO,  equipoVisitanteDTO, estadioDTO);
        Partido partido = _partidoRepositorio.ObtenerPartidos().FirstOrDefault();
        int id = partido.Id;
        PartidoDTO partidoPrueba = _servicioPartido.ObtenerPartido(id);
        Assert.AreEqual(partidoPrueba.equipoLocal.nombre, _partidoDTO.equipoLocal.nombre);
        Assert.AreEqual(partidoPrueba.equipoVisitante.nombre, _partidoDTO.equipoVisitante.nombre);
        Assert.AreEqual(partidoPrueba.Fecha, _partidoDTO.Fecha);
    }
}