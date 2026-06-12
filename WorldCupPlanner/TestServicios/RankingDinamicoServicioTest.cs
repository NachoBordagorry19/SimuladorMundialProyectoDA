using Dominio.Clases;
using Dominio.Enums;
using Moq;
using Repositorio;
using Repositorio.Interfaces;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class RankingDinamicoServicioTest
{
    private SqlContexto _contexto;
    private IEquipoRepositorio _equipoRepositorio;
    private RankingDinamicoServicio _servicioRankingDinamico;
    private Mock<IServicioAuditoria> _auditoriasMock;

    [TestInitialize]
    public void IniciarPrueba()
    {
        FabricaDeContextoDeAppEnMemoria fabricaDeContextoDeAppEnMemoria = new FabricaDeContextoDeAppEnMemoria();
        _contexto = fabricaDeContextoDeAppEnMemoria.CrearDbContexto();
        _equipoRepositorio = new EquipoRepositorioSql(_contexto);
        _auditoriasMock = new Mock<IServicioAuditoria>();
        _servicioRankingDinamico = new RankingDinamicoServicio(_equipoRepositorio, _auditoriasMock.Object);
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
            EquipoLocal = new EquipoDTO { Nombre = nombreLocal },
            EquipoVisitante = new EquipoDTO { Nombre = nombreVisitante },
            GolesLocal = golesLocal,
            GolesVisitante = golesVisitante,
            Fase = fase
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

    [TestMethod]
    public void ActualizarRanking_LocalGanaContraEquipoIgual_RankingLocalAumentaYVisitanteDisminuye()
    {
        AgregarEquipos("Barcelona",1500,"Bayern",1500);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona","Bayern",2,0,Fase.Grupos);
        _servicioRankingDinamico.ActualizarRanking(partidoDto);
        var localActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Barcelona");
        var visitanteActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Bayern");
        Assert.AreEqual(1515, localActualizado.RankingFifa);
        Assert.AreEqual(1485, visitanteActualizado.RankingFifa);
    }
    
    [TestMethod]
    public void ActualizarRanking_FaseEliminatoria_UsaMultiplicador1punto5()
    {
        AgregarEquipos("Barcelona", 1500, "Bayern", 1500);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona", "Bayern", 1, 0, Fase.Cuartos);

        _servicioRankingDinamico.ActualizarRanking(partidoDto);

        var localActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Barcelona");
        var visitanteActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Bayern");
        Assert.AreEqual(1523, localActualizado.RankingFifa);
        Assert.AreEqual(1478, visitanteActualizado.RankingFifa);
    }

    [TestMethod]
    public void ActualizarRanking_RankingNuevoSuperaMaximo_SeLimita2500()
    {
        AgregarEquipos("Barcelona", 2490, "Bayern", 2490);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona", "Bayern", 1, 0, Fase.Semifinal);

        _servicioRankingDinamico.ActualizarRanking(partidoDto);

        var localActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Barcelona");
        var visitanteActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Bayern");
        Assert.AreEqual(2500, localActualizado.RankingFifa);
        Assert.AreEqual(2468, visitanteActualizado.RankingFifa);
    }

    [TestMethod]
    public void ActualizarRanking_RankingNuevoBajaDeMinimo_SeLimita300()
    {
        AgregarEquipos("Barcelona", 300, "Bayern", 300);
        PartidoDTO partidoDto = CrearPartidoDTO("Barcelona", "Bayern", 0, 1, Fase.Grupos);

        _servicioRankingDinamico.ActualizarRanking(partidoDto);

        var localActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Barcelona");
        var visitanteActualizado = _equipoRepositorio.ObtenerEquipo(e => e.Nombre == "Bayern");
        Assert.AreEqual(300, localActualizado.RankingFifa);
        Assert.AreEqual(315, visitanteActualizado.RankingFifa);
    }

}