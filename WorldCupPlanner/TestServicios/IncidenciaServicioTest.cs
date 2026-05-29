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
        _incidenciaServicio.AgregarIncidencia(null);
    }

    [TestMethod]
    public void ObtenerIncidenciasPorPartido_SeObtienenCorrectamente()
    {
        List<Incidencia> incidencias = _incidenciaServicio.ObtenerIncidenciasPorPartido(2);
        Assert.AreEqual(1, incidencias.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerIncidenciasPorPartidoSiNegativo_LanzoExcepcion()
    {
        List<Incidencia> incidencias = _incidenciaServicio.ObtenerIncidenciasPorPartido(-2);
    }

    [TestMethod]
    public void EliminarIncidencia_SeEliminaCorrectamente()
    {
        List<Incidencia> incidenciasIniciales = _incidenciaServicio.ObtenerIncidenciasPorPartido(2);
        int countInicial = incidenciasIniciales.Count;
        _incidenciaServicio.EliminarIncidencia(5);
        List<Incidencia> incidenciasFinales = _incidenciaServicio.ObtenerIncidenciasPorPartido(2);
        int countFinal = incidenciasFinales.Count;
        Assert.AreEqual(countInicial - 1, countFinal);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarIncidenciaSiIdMenorA0_LanzoExcecpion()
    {
        _incidenciaServicio.EliminarIncidencia(-2);
    }
}