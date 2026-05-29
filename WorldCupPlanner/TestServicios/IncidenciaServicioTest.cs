using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class IncidenciaServicioTest
{
    private SqlContexto _contexto;
    private FabricaDeContextoDeAppEnMemoria _contextoFabrica;
    private IIncidenciaRepositorio _incidenciaRepositorio;
    private IncidenciaServicio  _incidenciaServicio;
    private IncidenciaDTO  _incidenciaDTO;
    private Incidencia _incidencia;

    [TestInitialize]
    public void IniciarPrueba()
    {
        _contextoFabrica = new FabricaDeContextoDeAppEnMemoria();
        _contexto = _contextoFabrica.CrearDbContexto();
        _contexto.Incidencias.RemoveRange(_contexto.Incidencias);
        _contexto.SaveChanges();
        _incidenciaRepositorio = new IncidenciaRepositorio(_contexto);
        _incidenciaServicio = new IncidenciaServicio(_incidenciaRepositorio);
        
        _incidencia = new Incidencia(2, 2, TipoIncidencia.TarjetaAmarilla);
        _incidencia.Id = 5; 
        _contexto.Incidencias.Add(_incidencia);
        _contexto.SaveChanges();
        
        _incidenciaDTO = new IncidenciaDTO()
        {
            Id = 1, 
            IdEquipo = 2,
            IdPartido = 2,
            TipoIncidencia = TipoIncidencia.TarjetaAmarilla
        };
    }

    [TestMethod]
    public void AgregarIncidenciSiNoExiste_SeAgregarCorrectamente()
    {
        _incidenciaDTO.Id = 0;
        _incidenciaServicio.AgregarIncidencia(_incidenciaDTO);
        Assert.AreEqual(2, _incidenciaDTO.IdEquipo);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarIncidenciaSiExiste_LanzoExcepcion()
    {
        _incidenciaDTO.Id = 5; 
        _incidenciaServicio.AgregarIncidencia(_incidenciaDTO);
        _incidenciaServicio.AgregarIncidencia(_incidenciaDTO);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarIncidenciaSiEsNull_LanzoExcepcion()
    {
        _incidenciaRepositorio.AgregarIncidencia(null);
    }
}