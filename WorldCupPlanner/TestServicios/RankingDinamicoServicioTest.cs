using Dominio.Clases;
using Dominio.Enums;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class RankingDinamicoServicioTest
{
    private SqlContexto _contexto;
    private IEquipoRepositorio _equipoRepositorio;
    private RankingDinamicoServicio _servicioRankingDinamico;

    [TestInitialize]
    public void IniciarPrueba()
    {
        FabricaDeContextoDeAppEnMemoria fabricaDeContextoDeAppEnMemoria = new FabricaDeContextoDeAppEnMemoria();
        _contexto = fabricaDeContextoDeAppEnMemoria.CrearDbContexto();
        _equipoRepositorio = new EquipoRepositorioSql(_contexto);
        _servicioRankingDinamico = new RankingDinamicoServicio(_equipoRepositorio);
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
        AgregarEquipos("Barcelona",301,"Bayern",302);
        PartidoDTO partidoDto = CrearPartidoDTO("Argentina","Bayern",1,2,Fase.Final);
        _servicioRankingDinamico.ActualizarRanking(partidoDto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarRanking_EquipoVisitanteNoExisteEnSistema_LanzaExcepcion()
    {
        AgregarEquipos("Barcelona",301,"Bayern",302);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona","Argentina",1,2,Fase.Final);
        _servicioRankingDinamico.ActualizarRanking(partidoDto);
    }

    [TestMethod]
    public void ActualizarRanking_EmpateEntreEquiposIguales_RankingNoVaria()
    {
        AgregarEquipos("Barcelona",301,"Bayern",302);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona","Bayern",1,1,Fase.Grupos);
        _servicioRankingDinamico.ActualizarRanking(partidoDto);
        var localActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Barcelona");
        var visitanteActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Bayern");
        Assert.AreEqual(301, localActualizado.RankingFifa);
        Assert.AreEqual(302, visitanteActualizado.RankingFifa);

    }

}