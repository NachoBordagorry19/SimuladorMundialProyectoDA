using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;

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
    
}
