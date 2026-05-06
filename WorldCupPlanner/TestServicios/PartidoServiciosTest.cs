using Repositorio;
using Repositorio.Interfaces;

namespace TestServicios;

[TestClass]
public class PartidoServiciosTest
{
    private BaseDeDatosEnMemoria _baseDeDatosEnMemoria;
    private IPartidoRepositorio _partidoRepositorio;
    private IServicioPartido _servicioPartido;
    private PartidoDTO _partidoDTO;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatosEnMemoria = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatosEnMemoria);
        _servicioPartido = new ServicioPartido(_partidoRepositorio);
        
    }

    [TestMethod]
    public void AgregarPartido()
    {
        _servicioPartido.AgregarPartido(_partidoDTO);
        Assert.AreEqual("Bayern", _partidoDTO.Local);
    }
    
}