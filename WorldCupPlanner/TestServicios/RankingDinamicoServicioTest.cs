using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class RankingDinamicoServicioTest
{
    private SqlContexto _contexto;
    private IEquipoRepositorio _equipoRepositorio;
    private ServicioRankingDinamico _servicioRankingDinamico;

    [TestInitialize]
    public void IniciarPrueba()
    {
        FabricaDeContextoDeAppEnMemoria fabricaDeContextoDeAppEnMemoria = new FabricaDeContextoDeAppEnMemoria();
        _contexto = fabricaDeContextoDeAppEnMemoria.CrearDbContexto();
        _equipoRepositorio = new EquipoRepositorioSql(_contexto);
        _servicioRankingDinamico = new ServicioRankingDinamico(_equipoRepositorio);
    }
    
    private void AgregarEquipos(string nombreLocal, int rankingLocal, string nombreVisitante, int rankingVisitante)
    {
        _contexto.Equipos.Add(new Equipo(nombreLocal, Confederacion.UEFA, rankingLocal));
        _contexto.Equipos.Add(new Equipo(nombreVisitante, Confederacion.UEFA, rankingVisitante));
        _contexto.SaveChanges();
    }

    private PartidoDTO CrearPartidoDTO(string nombreLocal, string nombreVisitante, int golesLocal, int golesVisitante,Fase fase)
    {
        return new PartidoDTO
        {
            equipoLocal = new EquipoDTO { nombre = nombreLocal },
            equipoVisitante = new EquipoDTO { nombre = nombreVisitante },
            golesLocal = golesLocal,
            golesVisitante = golesVisitante,
            fase = fase
        };
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarRanking_EquipoLocalNoExisteEnSistema_LanzaExcepcion()
    {
        AgregarEquipos("Barcelona",49,"Bayern",48);
        PartidoDTO partidoDto = CrearPartidoDTO("Argentina","Bayern",1,2,Fase.Final);
        _servicioRankingDinamico.ActualizarRanking(partidoDto);
    }
}