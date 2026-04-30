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
        
        EquipoDTO _equipoDTO2 = new EquipoDTO()
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
}
