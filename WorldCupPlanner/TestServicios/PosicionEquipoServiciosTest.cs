using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class PosicionEquipoServiciosTest
{
    private BaseDeDatosEnMemoria _BDEnMemoria;
    private IServicioPosicionEquipo _servicioPosicionEquipo;
    private IPosicionEquipoRepositorio _posicionEquipoRepositorio;
    private PosicionEquipoDTO _posicionEquipoDTO;

    [TestInitialize]
    public void TestInitialize()
    {
        _BDEnMemoria = new BaseDeDatosEnMemoria();
        _posicionEquipoRepositorio = new PosicionEquipoRepositorio(_BDEnMemoria);
        _servicioPosicionEquipo = new PosicionEquipoServicios(_posicionEquipoRepositorio);
        _posicionEquipoDTO = new PosicionEquipoDTO()
        {
            EquipoNombre = "Uruguay",
            PartidosJugados = 0,
            Ganados = 0,
            Empatados = 0,
            Perdidos = 0,
            GolesAFavor = 0,
            GolesEnContra = 0,
            Diferencia = 0,
            Puntos = 0
        };
    }

    [TestMethod]
    public void AgregarPosicion_DatosInicialesCorrectos()
    {
        _servicioPosicionEquipo.AgregarPosicion(_posicionEquipoDTO);
        var obtenido = _servicioPosicionEquipo.ObtenerPosicionPorEquipo("Uruguay");
        Assert.AreEqual("Uruguay", obtenido.EquipoNombre);
        Assert.AreEqual(0, obtenido.Puntos);
        Assert.AreEqual(0, obtenido.PartidosJugados);
    }
}