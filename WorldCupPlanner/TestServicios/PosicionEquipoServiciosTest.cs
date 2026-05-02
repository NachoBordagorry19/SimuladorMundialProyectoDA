using Repositorio;
using Repositorio.Interfaces;
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
}