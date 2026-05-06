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
    }

    [TestMethod]
    public void AgregarPartido()
    {

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
}