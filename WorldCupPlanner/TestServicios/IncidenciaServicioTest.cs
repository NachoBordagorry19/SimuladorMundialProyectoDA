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
        _incidenciaDTO = new IncidenciaDTO()
        {
            Id = _incidencia.Id,
            IdEquipo = 2,
            IdPartido = 2,
            TipoIncidencia = TipoIncidencia.TarjetaAmarilla
        };
    }

    [TestMethod]
    public void AgregarIncidenciSiNoExiste_SeAgregarCorrectamente()
    {
        _incidenciaServicio.AgregarIncidencia(_incidenciaDTO);
        Assert.AreEqual(2,_incidenciaDTO.IdEquipo);
    }
}