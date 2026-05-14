using Dominio.Enums;
using Moq;
using Repositorio;
using Servicios.Clases;
using Servicios.Interfaces;
using Servicios.Modelo;

namespace TestServicios;

[TestClass]
public class CrucesSegundaFaseServicioTest
{
    private BaseDeDatosEnMemoria _baseDeDatos;
    private PartidoRepositorio _partidoRepositorio;
    private IServicioPartido _partidoServicios;
    private CrucesSegundaFaseServicio _crucesServicio;
    private Mock<IServicioAuditoria> _auditoriaMock;

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatos = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatos);
        _auditoriaMock = new Mock<IServicioAuditoria>();
        _partidoServicios = new PartidoServicios(_partidoRepositorio, _auditoriaMock.Object);
        _crucesServicio = new CrucesSegundaFaseServicio(_partidoServicios, _auditoriaMock.Object);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_OrdenaEquiposPorPuntos()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 2, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoA, equipoD, estadio, 3, 0);

        AgregarPartido(equipoB, equipoC, estadio, 2, 0);
        AgregarPartido(equipoB, equipoD, estadio, 1, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo A", ranking[0].EquipoNombre);
        Assert.AreEqual(9, ranking[0].Puntos);

        Assert.AreEqual("Equipo B", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);

        Assert.AreEqual("Equipo C", ranking[2].EquipoNombre);
        Assert.AreEqual(3, ranking[2].Puntos);

        Assert.AreEqual("Equipo D", ranking[3].EquipoNombre);
        Assert.AreEqual(0, ranking[3].Puntos);
    }

    private EquipoDTO CrearEquipo(string nombre)
    {
        return new EquipoDTO
        {
            nombre = nombre,
            confederacion = Confederacion.UEFA,
            rankingFifa = 1500
        };
    }

    private EstadioDTO CrearEstadio()
    {
        return new EstadioDTO
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "Estadio Centenario",
            CapacidadLocativa = 60000
        };
    }

    private void AgregarPartido(
        EquipoDTO local,
        EquipoDTO visitante,
        EstadioDTO estadio,
        int golesLocal,
        int golesVisitante)
    {
        var partido = new PartidoDTO
        {
            Grupo = "A",
            Fecha = new DateTime(2026, 06, 01),
            Estadio = estadio,
            equipoLocal = local,
            equipoVisitante = visitante,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = golesLocal,
            golesVisitante = golesVisitante
        };

        _partidoServicios.AgregarPartido(partido, local, visitante, estadio);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_SiEmpatanEnPuntos_OrdenaPorDiferenciaDeGoles()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 1, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoD, equipoA, estadio, 1, 0);

        AgregarPartido(equipoB, equipoC, estadio, 5, 0);
        AgregarPartido(equipoB, equipoD, estadio, 5, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo B", ranking[0].EquipoNombre);
        Assert.AreEqual(6, ranking[0].Puntos);
        Assert.AreEqual(9, ranking[0].Diferencia);

        Assert.AreEqual("Equipo A", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);
        Assert.AreEqual(1, ranking[1].Diferencia);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_SiEmpatanEnPuntosYDiferencia_OrdenaPorGolesAFavor()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 0, 2);
        AgregarPartido(equipoA, equipoC, estadio, 2, 0);
        AgregarPartido(equipoA, equipoD, estadio, 2, 0);

        AgregarPartido(equipoB, equipoC, estadio, 4, 0);
        AgregarPartido(equipoB, equipoD, estadio, 0, 4);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual("Equipo B", ranking[0].EquipoNombre);
        Assert.AreEqual(6, ranking[0].Puntos);
        Assert.AreEqual(2, ranking[0].Diferencia);
        Assert.AreEqual(6, ranking[0].GolesAFavor);

        Assert.AreEqual("Equipo A", ranking[1].EquipoNombre);
        Assert.AreEqual(6, ranking[1].Puntos);
        Assert.AreEqual(2, ranking[1].Diferencia);
        Assert.AreEqual(4, ranking[1].GolesAFavor);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_SiPersisteEmpate_UsaSorteoDeterministico()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 0, 0);
        AgregarPartido(equipoA, equipoC, estadio, 0, 0);
        AgregarPartido(equipoA, equipoD, estadio, 0, 0);

        AgregarPartido(equipoB, equipoC, estadio, 0, 0);
        AgregarPartido(equipoB, equipoD, estadio, 0, 0);

        AgregarPartido(equipoC, equipoD, estadio, 0, 0);

        int semillaCrucesFase = 123;
        int otraSemillaCrucesFase = 456;

        var rankingSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", semillaCrucesFase);
        var rankingMismaSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", semillaCrucesFase);
        var rankingOtraSemillaCrucesFase = _crucesServicio.ObtenerRankingGrupo("A", otraSemillaCrucesFase);

        var ordenSemillaCrucesFase = rankingSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();
        var ordenMismaSemillaCrucesFase = rankingMismaSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();
        var ordenOtraSemillaCrucesFase = rankingOtraSemillaCrucesFase.Select(e => e.EquipoNombre).ToList();

        CollectionAssert.AreEqual(ordenSemillaCrucesFase, ordenMismaSemillaCrucesFase);

        bool mismoOrdenConDistintaSemilla = true;

        for (int i = 0; i < ordenSemillaCrucesFase.Count; i++)
        {
            if (ordenSemillaCrucesFase[i] != ordenOtraSemillaCrucesFase[i])
            {
                mismoOrdenConDistintaSemilla = false;
            }
        }

        Assert.IsFalse(mismoOrdenConDistintaSemilla);
    }

    [TestMethod]
    public void ObtenerRankingGrupo_AsignaGrupoALasPosiciones()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var equipoC = CrearEquipo("Equipo C");
        var equipoD = CrearEquipo("Equipo D");
        var estadio = CrearEstadio();

        AgregarPartido(equipoA, equipoB, estadio, 2, 0);
        AgregarPartido(equipoA, equipoC, estadio, 1, 0);
        AgregarPartido(equipoA, equipoD, estadio, 3, 0);

        AgregarPartido(equipoB, equipoC, estadio, 2, 0);
        AgregarPartido(equipoB, equipoD, estadio, 1, 0);

        AgregarPartido(equipoC, equipoD, estadio, 1, 0);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        foreach (var posicion in ranking)
        {
            Assert.AreEqual("A", posicion.Grupo);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ActualizarPartido_DespuesDeGenerarCruces_NoPermiteEditarFaseGrupos()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var estadio = CrearEstadio();

        var partido = new PartidoDTO
        {
            Grupo = "A",
            Fecha = new DateTime(2026, 06, 01),
            Estadio = estadio,
            equipoLocal = equipoA,
            equipoVisitante = equipoB,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 1,
            golesVisitante = 0
        };

        _partidoServicios.AgregarPartido(partido, equipoA, equipoB, estadio);

        var clasificados = new ClasificadosDTO();

        for (int i = 1; i <= 12; i++)
        {
            clasificados.Primeros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Primero " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 30 - i
            });

            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 20 - i
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Terceros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Tercero " + i,
                Grupo = ((char)('A' + i + 3)).ToString(),
                Puntos = 8 - i
            });
        }

        _crucesServicio.GenerarCrucesFase(clasificados, 123);

        partido.Fecha = new DateTime(2026, 06, 02);

        _partidoServicios.ActualizarPartido(partido);
    }

    [TestMethod]
    public void ObtenerClasificados_SeleccionaPrimerosSegundosYOchoMejoresTerceros()
    {
        var estadio = new EstadioDTO
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "Estadio Centenario",
            CapacidadLocativa = 60000
        };

        for (char letraGrupo = 'A'; letraGrupo <= 'L'; letraGrupo++)
        {
            string grupo = letraGrupo.ToString();
            bool tercerEquipoDebeClasificar = letraGrupo <= 'H';

            var equipo1 = new EquipoDTO
            {
                nombre = grupo + " Equipo 1",
                confederacion = Confederacion.UEFA,
                rankingFifa = 2000
            };

            var equipo2 = new EquipoDTO
            {
                nombre = grupo + " Equipo 2",
                confederacion = Confederacion.CONMEBOL,
                rankingFifa = 1900
            };

            var equipo3 = new EquipoDTO
            {
                nombre = grupo + " Equipo 3",
                confederacion = Confederacion.CAF,
                rankingFifa = 1800
            };

            var equipo4 = new EquipoDTO
            {
                nombre = grupo + " Equipo 4",
                confederacion = Confederacion.AFC,
                rankingFifa = 1700
            };

            var partido1 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo1,
                equipoVisitante = equipo2,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = 3,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido1, equipo1, equipo2, estadio);

            var partido2 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo1,
                equipoVisitante = equipo3,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = tercerEquipoDebeClasificar ? 3 : 5,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido2, equipo1, equipo3, estadio);

            var partido3 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo1,
                equipoVisitante = equipo4,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = tercerEquipoDebeClasificar ? 3 : 1,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido3, equipo1, equipo4, estadio);

            var partido4 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo2,
                equipoVisitante = equipo3,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = tercerEquipoDebeClasificar ? 2 : 4,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido4, equipo2, equipo3, estadio);

            var partido5 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo2,
                equipoVisitante = equipo4,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = tercerEquipoDebeClasificar ? 2 : 1,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido5, equipo2, equipo4, estadio);

            var partido6 = new PartidoDTO
            {
                Grupo = grupo,
                Fecha = new DateTime(2026, 06, 01),
                Estadio = estadio,
                equipoLocal = equipo3,
                equipoVisitante = equipo4,
                fase = Fase.Grupos,
                estadoPartido = EstadoPartido.Jugado,
                golesLocal = tercerEquipoDebeClasificar ? 1 : 0,
                golesVisitante = 0
            };
            _partidoServicios.AgregarPartido(partido6, equipo3, equipo4, estadio);
        }

        var clasificados = _crucesServicio.ObtenerClasificados(123);

        Assert.AreEqual(12, clasificados.Primeros.Count);
        Assert.AreEqual(12, clasificados.Segundos.Count);
        Assert.AreEqual(8, clasificados.Terceros.Count);

        int totalClasificados = clasificados.Primeros.Count
                                + clasificados.Segundos.Count
                                + clasificados.Terceros.Count;

        Assert.AreEqual(32, totalClasificados);

        var nombresTerceros = clasificados.Terceros
            .Select(t => t.EquipoNombre)
            .ToList();

        Assert.IsTrue(nombresTerceros.Contains("A Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("B Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("C Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("D Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("E Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("F Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("G Equipo 3"));
        Assert.IsTrue(nombresTerceros.Contains("H Equipo 3"));

        Assert.IsFalse(nombresTerceros.Contains("I Equipo 3"));
        Assert.IsFalse(nombresTerceros.Contains("J Equipo 3"));
        Assert.IsFalse(nombresTerceros.Contains("K Equipo 3"));
        Assert.IsFalse(nombresTerceros.Contains("L Equipo 3"));
    }

    [TestMethod]
    public void GenerarCrucesPrimeraRonda_GeneraOchoCrucesEntreMejoresPrimerosYTerceros()
    {
        var primeros = new List<PosicionEquipoDTO>();
        var terceros = new List<PosicionEquipoDTO>();

        for (int i = 1; i <= 8; i++)
        {
            primeros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Primero " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 9
            });

            terceros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Tercero " + i,
                Grupo = ((char)('I' + i - 1)).ToString(),
                Puntos = 4
            });
        }

        var cruces = _crucesServicio.GenerarCrucesEntreListas(primeros, terceros, "A", 123);

        Assert.AreEqual(8, cruces.Count);

        for (int i = 0; i < 8; i++)
        {
            Assert.AreEqual("A" + (i + 1), cruces[i].Codigo);
            Assert.IsTrue(cruces[i].EquipoLocal.EquipoNombre.StartsWith("Primero"));
            Assert.IsTrue(cruces[i].EquipoVisitante.EquipoNombre.StartsWith("Tercero"));
            Assert.AreNotEqual(cruces[i].EquipoLocal.Grupo, cruces[i].EquipoVisitante.Grupo);
        }
    }

    [TestMethod]
    public void GenerarCrucesFase_ConClasificados_GeneraCrucesA1HastaA8()
    {
        var clasificados = new ClasificadosDTO();

        for (int i = 1; i <= 12; i++)
        {
            clasificados.Primeros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Primero " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 20 - i
            });
        }

        for (int i = 1; i <= 12; i++)
        {
            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 12 - i
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Terceros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Tercero " + i,
                Grupo = ((char)('A' + i + 3)).ToString(),
                Puntos = 8 - i
            });
        }

        var cruces = _crucesServicio.GenerarCrucesFase(clasificados, 123);

        var crucesA = cruces
            .Where(c => c.Codigo.StartsWith("A"))
            .ToList();

        Assert.AreEqual(8, crucesA.Count);

        for (int i = 1; i <= 8; i++)
        {
            Assert.IsTrue(crucesA.Any(c => c.Codigo == "A" + i));
        }

        foreach (var cruce in crucesA)
        {
            Assert.IsTrue(cruce.EquipoLocal.EquipoNombre.StartsWith("Primero"));
            Assert.IsTrue(cruce.EquipoVisitante.EquipoNombre.StartsWith("Tercero"));
            Assert.AreNotEqual(cruce.EquipoLocal.Grupo, cruce.EquipoVisitante.Grupo);
            Assert.AreEqual(Fase.Dieciseisavos, cruce.Fase);
        }
    }

    [TestMethod]
    public void GenerarCrucesFase_ConClasificados_GeneraCrucesB1HastaB4()
    {
        var clasificados = new ClasificadosDTO();

        for (int i = 1; i <= 12; i++)
        {
            clasificados.Primeros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Primero " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 30 - i
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 20 - i
            });
        }

        for (int i = 9; i <= 12; i++)
        {
            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 9)).ToString(),
                Puntos = 1
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Terceros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Tercero " + i,
                Grupo = ((char)('A' + i + 3)).ToString(),
                Puntos = 8 - i
            });
        }

        var cruces = _crucesServicio.GenerarCrucesFase(clasificados, 123);

        var crucesB = cruces
            .Where(c => c.Codigo == "B1" || c.Codigo == "B2" || c.Codigo == "B3" || c.Codigo == "B4")
            .ToList();

        Assert.AreEqual(4, crucesB.Count);

        Assert.IsTrue(crucesB.Any(c => c.Codigo == "B1"));
        Assert.IsTrue(crucesB.Any(c => c.Codigo == "B2"));
        Assert.IsTrue(crucesB.Any(c => c.Codigo == "B3"));
        Assert.IsTrue(crucesB.Any(c => c.Codigo == "B4"));

        foreach (var cruce in crucesB)
        {
            Assert.IsTrue(cruce.EquipoLocal.EquipoNombre.StartsWith("Primero"));
            Assert.IsTrue(cruce.EquipoVisitante.EquipoNombre == "Segundo 9" ||
                          cruce.EquipoVisitante.EquipoNombre == "Segundo 10" ||
                          cruce.EquipoVisitante.EquipoNombre == "Segundo 11" ||
                          cruce.EquipoVisitante.EquipoNombre == "Segundo 12");

            Assert.AreNotEqual(cruce.EquipoLocal.Grupo, cruce.EquipoVisitante.Grupo);
        }
    }

    [TestMethod]
    public void GenerarCrucesFase_ConClasificados_GeneraCrucesB5HastaB8()
    {
        var clasificados = new ClasificadosDTO();

        for (int i = 1; i <= 12; i++)
        {
            clasificados.Primeros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Primero " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 30 - i
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 1)).ToString(),
                Puntos = 20 - i
            });
        }

        for (int i = 9; i <= 12; i++)
        {
            clasificados.Segundos.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Segundo " + i,
                Grupo = ((char)('A' + i - 9)).ToString(),
                Puntos = 1
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            clasificados.Terceros.Add(new PosicionEquipoDTO
            {
                EquipoNombre = "Tercero " + i,
                Grupo = ((char)('A' + i + 3)).ToString(),
                Puntos = 8 - i
            });
        }

        var cruces = _crucesServicio.GenerarCrucesFase(clasificados, 123);

        var crucesB5aB8 = cruces
            .Where(c => c.Codigo == "B5" || c.Codigo == "B6" || c.Codigo == "B7" || c.Codigo == "B8")
            .ToList();

        Assert.AreEqual(4, crucesB5aB8.Count);

        Assert.IsTrue(crucesB5aB8.Any(c => c.Codigo == "B5"));
        Assert.IsTrue(crucesB5aB8.Any(c => c.Codigo == "B6"));
        Assert.IsTrue(crucesB5aB8.Any(c => c.Codigo == "B7"));
        Assert.IsTrue(crucesB5aB8.Any(c => c.Codigo == "B8"));

        foreach (var cruce in crucesB5aB8)
        {
            Assert.IsTrue(cruce.EquipoLocal.EquipoNombre.StartsWith("Segundo"));
            Assert.IsTrue(cruce.EquipoVisitante.EquipoNombre.StartsWith("Segundo"));

            Assert.IsFalse(cruce.EquipoLocal.EquipoNombre == "Segundo 9");
            Assert.IsFalse(cruce.EquipoLocal.EquipoNombre == "Segundo 10");
            Assert.IsFalse(cruce.EquipoLocal.EquipoNombre == "Segundo 11");
            Assert.IsFalse(cruce.EquipoLocal.EquipoNombre == "Segundo 12");

            Assert.IsFalse(cruce.EquipoVisitante.EquipoNombre == "Segundo 9");
            Assert.IsFalse(cruce.EquipoVisitante.EquipoNombre == "Segundo 10");
            Assert.IsFalse(cruce.EquipoVisitante.EquipoNombre == "Segundo 11");
            Assert.IsFalse(cruce.EquipoVisitante.EquipoNombre == "Segundo 12");

            Assert.AreNotEqual(cruce.EquipoLocal.Grupo, cruce.EquipoVisitante.Grupo);
        }
    }

    [TestMethod]
    public void GenerarCrucesFase_DebeRegistrarAuditoria()
    {
        var clasificados = new ClasificadosDTO();
        for (int i = 1; i <= 12; i++)
        {
            clasificados.Primeros.Add(new PosicionEquipoDTO { EquipoNombre = $"P{i}", Grupo = "A" });
            clasificados.Segundos.Add(new PosicionEquipoDTO { EquipoNombre = $"S{i}", Grupo = "B" });
        }
        for (int i = 1; i <= 8; i++)
        {
            clasificados.Terceros.Add(new PosicionEquipoDTO { EquipoNombre = $"T{i}", Grupo = "C" });
        }

        _crucesServicio.GenerarCrucesFase(clasificados, 123);
        _auditoriaMock.Verify(a => a.RegistrarSorteoCruces(), Times.Once);
    }

    [TestMethod]
    public void GenerarOctavosDeFinal_GeneraC1HastaC8()
    {
        var crucesDieciseisavos = new List<CruceDTO>();

        for (int i = 1; i <= 8; i++)
        {
            crucesDieciseisavos.Add(new CruceDTO
            {
                Codigo = "A" + i,
                Fase = Fase.Dieciseisavos
            });
        }

        for (int i = 1; i <= 8; i++)
        {
            crucesDieciseisavos.Add(new CruceDTO
            {
                Codigo = "B" + i,
                Fase = Fase.Dieciseisavos
            });
        }

        var octavos = _crucesServicio.GenerarOctavosDeFinal(crucesDieciseisavos);

        Assert.AreEqual(8, octavos.Count);

        for (int i = 1; i <= 8; i++)
        {
            var cruce = octavos.First(c => c.Codigo == "C" + i);

            Assert.AreEqual(Fase.Octavos, cruce.Fase);
            Assert.AreEqual("Ganador A" + i, cruce.ReferenciaLocal);
            Assert.AreEqual("Ganador B" + i, cruce.ReferenciaVisitante);
        }
    }

    [TestMethod]
    public void GenerarCuartosDeFinal_GeneraD1HastaD4()
    {
        var octavos = new List<CruceDTO>();

        for (int i = 1; i <= 8; i++)
        {
            octavos.Add(new CruceDTO
            {
                Codigo = "C" + i,
                Fase = Fase.Octavos,
                ReferenciaLocal = "Ganador A" + i,
                ReferenciaVisitante = "Ganador B" + i
            });
        }

        var cuartos = _crucesServicio.GenerarCuartosDeFinal(octavos);

        Assert.AreEqual(4, cuartos.Count);

        var d1 = cuartos.First(c => c.Codigo == "D1");
        Assert.AreEqual(Fase.Cuartos, d1.Fase);
        Assert.AreEqual("Ganador C1", d1.ReferenciaLocal);
        Assert.AreEqual("Ganador C2", d1.ReferenciaVisitante);

        var d2 = cuartos.First(c => c.Codigo == "D2");
        Assert.AreEqual(Fase.Cuartos, d2.Fase);
        Assert.AreEqual("Ganador C3", d2.ReferenciaLocal);
        Assert.AreEqual("Ganador C4", d2.ReferenciaVisitante);

        var d3 = cuartos.First(c => c.Codigo == "D3");
        Assert.AreEqual(Fase.Cuartos, d3.Fase);
        Assert.AreEqual("Ganador C5", d3.ReferenciaLocal);
        Assert.AreEqual("Ganador C6", d3.ReferenciaVisitante);

        var d4 = cuartos.First(c => c.Codigo == "D4");
        Assert.AreEqual(Fase.Cuartos, d4.Fase);
        Assert.AreEqual("Ganador C7", d4.ReferenciaLocal);
        Assert.AreEqual("Ganador C8", d4.ReferenciaVisitante);
    }

    [TestMethod]
    public void GenerarSemifinales_GeneraS1YS2()
    {
        var cuartos = new List<CruceDTO>();

        for (int i = 1; i <= 4; i++)
        {
            cuartos.Add(new CruceDTO
            {
                Codigo = "D" + i,
                Fase = Fase.Cuartos,
                ReferenciaLocal = "Ganador C" + ((i * 2) - 1),
                ReferenciaVisitante = "Ganador C" + (i * 2)
            });
        }

        var semifinales = _crucesServicio.GenerarSemifinales(cuartos);

        Assert.AreEqual(2, semifinales.Count);

        var s1 = semifinales.First(c => c.Codigo == "S1");
        Assert.AreEqual(Fase.Semifinal, s1.Fase);
        Assert.AreEqual("Ganador D1", s1.ReferenciaLocal);
        Assert.AreEqual("Ganador D2", s1.ReferenciaVisitante);

        var s2 = semifinales.First(c => c.Codigo == "S2");
        Assert.AreEqual(Fase.Semifinal, s2.Fase);
        Assert.AreEqual("Ganador D3", s2.ReferenciaLocal);
        Assert.AreEqual("Ganador D4", s2.ReferenciaVisitante);
    }

    [TestMethod]
    public void GenerarTercerPuestoYFinal_GeneraPartidoTercerPuestoYFinal()
    {
        var semifinales = new List<CruceDTO>
        {
            new CruceDTO
            {
                Codigo = "S1",
                Fase = Fase.Semifinal,
                ReferenciaLocal = "Ganador D1",
                ReferenciaVisitante = "Ganador D2"
            },
            new CruceDTO
            {
                Codigo = "S2",
                Fase = Fase.Semifinal,
                ReferenciaLocal = "Ganador D3",
                ReferenciaVisitante = "Ganador D4"
            }
        };

        var partidosFinales = _crucesServicio.GenerarTercerPuestoYFinal(semifinales);

        Assert.AreEqual(2, partidosFinales.Count);

        var tercerPuesto = partidosFinales.First(c => c.Codigo == "TercerPuesto");
        Assert.AreEqual(Fase.Tercero, tercerPuesto.Fase);
        Assert.AreEqual("Perdedor S1", tercerPuesto.ReferenciaLocal);
        Assert.AreEqual("Perdedor S2", tercerPuesto.ReferenciaVisitante);

        var final = partidosFinales.First(c => c.Codigo == "Final");
        Assert.AreEqual(Fase.Final, final.Fase);
        Assert.AreEqual("Ganador S1", final.ReferenciaLocal);
        Assert.AreEqual("Ganador S2", final.ReferenciaVisitante);
    }
    [TestMethod]
    public void ObtenerCampeon_FinalJugada_RetornaGanador()
    {
        var equipoLocal = new EquipoDTO
        {
            nombre = "Uruguay",
            confederacion = Confederacion.CONMEBOL,
            rankingFifa = 2000
        };

        var equipoVisitante = new EquipoDTO
        {
            nombre = "Brasil",
            confederacion = Confederacion.CONMEBOL,
            rankingFifa = 2100
        };

        var estadio = new EstadioDTO
        {
            Nombre = "Centenario",
            Ciudad = "Montevideo",
            Descripcion = "Estadio Centenario",
            CapacidadLocativa = 60000
        };

        var final = new PartidoDTO
        {
            Fecha = new DateTime(2026, 07, 19),
            Estadio = estadio,
            equipoLocal = equipoLocal,
            equipoVisitante = equipoVisitante,
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 2,
            golesVisitante = 1
        };

        var campeon = _crucesServicio.ObtenerCampeon(final);

        Assert.AreEqual("Uruguay", campeon.nombre);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerClasificados_SiFaltanPartidosJugados_LanzaExcepcion()
    {
        _crucesServicio.ObtenerClasificados(123);
    }
    
    [TestMethod]
    public void ObtenerRankingGrupo_SiPartidoEstaPendiente_NoSumaPuntos()
    {
        var equipoA = CrearEquipo("Equipo A");
        var equipoB = CrearEquipo("Equipo B");
        var estadio = CrearEstadio();

        var partido = new PartidoDTO
        {
            Grupo = "A",
            Fecha = new DateTime(2026, 06, 01),
            Estadio = estadio,
            equipoLocal = equipoA,
            equipoVisitante = equipoB,
            fase = Fase.Grupos,
            estadoPartido = EstadoPartido.Pendiente
        };

        _partidoServicios.AgregarPartido(partido, equipoA, equipoB, estadio);

        var ranking = _crucesServicio.ObtenerRankingGrupo("A", 123);

        Assert.AreEqual(2, ranking.Count);
        Assert.AreEqual(0, ranking[0].Puntos);
        Assert.AreEqual(0, ranking[1].Puntos);
    }
    [TestMethod]
    public void GenerarCrucesEntreListas_SiNoHayVisitanteDeOtroGrupo_UsaElPrimero()
    {
        var local = new PosicionEquipoDTO { EquipoNombre = "Local", Grupo = "A" };
        var visitante = new PosicionEquipoDTO { EquipoNombre = "Visitante", Grupo = "A" };

        var cruces = _crucesServicio.GenerarCrucesEntreListas(
            new List<PosicionEquipoDTO> { local },
            new List<PosicionEquipoDTO> { visitante },
            "A",
            123
        );

        Assert.AreEqual("Visitante", cruces[0].EquipoVisitante.EquipoNombre);
    }
    
    [TestMethod]
    public void ObtenerCampeon_SiGanaVisitante_RetornaVisitante()
    {
        var final = new PartidoDTO
        {
            equipoLocal = CrearEquipo("Uruguay"),
            equipoVisitante = CrearEquipo("Brasil"),
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 1,
            golesVisitante = 2
        };

        var campeon = _crucesServicio.ObtenerCampeon(final);

        Assert.AreEqual("Brasil", campeon.nombre);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerCampeon_SiFinalEmpata_LanzaExcepcion()
    {
        var final = new PartidoDTO
        {
            equipoLocal = CrearEquipo("Uruguay"),
            equipoVisitante = CrearEquipo("Brasil"),
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 1,
            golesVisitante = 1
        };

        _crucesServicio.ObtenerCampeon(final);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerCampeon_SiNoEsFinal_LanzaExcepcion()
    {
        var partido = new PartidoDTO
        {
            equipoLocal = CrearEquipo("Uruguay"),
            equipoVisitante = CrearEquipo("Brasil"),
            fase = Fase.Semifinal,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 2,
            golesVisitante = 1
        };

        _crucesServicio.ObtenerCampeon(partido);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerCampeon_SiFinalNoEstaJugada_LanzaExcepcion()
    {
        var final = new PartidoDTO
        {
            equipoLocal = CrearEquipo("Uruguay"),
            equipoVisitante = CrearEquipo("Brasil"),
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Pendiente,
            golesLocal = 0,
            golesVisitante = 0
        };

        _crucesServicio.ObtenerCampeon(final);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ObtenerCampeon_SiFinalEsNula_LanzaExcepcion()
    {
        _crucesServicio.ObtenerCampeon(null);
    }
    
    [TestMethod]
    public void ObtenerCampeonActual_SiNoHayFinal_RetornaNull()
    {
        var campeon = _crucesServicio.ObtenerCampeonActual();

        Assert.IsNull(campeon);
    }
    
    [TestMethod]
    public void ObtenerCampeonActual_SiFinalEstaPendiente_RetornaNull()
    {
        var local = CrearEquipo("Uruguay");
        var visitante = CrearEquipo("Brasil");
        var estadio = CrearEstadio();

        var final = new PartidoDTO
        {
            Fecha = new DateTime(2026, 07, 19),
            Estadio = estadio,
            equipoLocal = local,
            equipoVisitante = visitante,
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Pendiente
        };

        _partidoServicios.AgregarPartido(final, local, visitante, estadio);

        var campeon = _crucesServicio.ObtenerCampeonActual();

        Assert.IsNull(campeon);
    }
    
    [TestMethod]
    public void ObtenerCampeonActual_SiFinalEstaJugada_RetornaCampeon()
    {
        var local = CrearEquipo("Uruguay");
        var visitante = CrearEquipo("Brasil");
        var estadio = CrearEstadio();

        var final = new PartidoDTO
        {
            Fecha = new DateTime(2026, 07, 19),
            Estadio = estadio,
            equipoLocal = local,
            equipoVisitante = visitante,
            fase = Fase.Final,
            estadoPartido = EstadoPartido.Jugado,
            golesLocal = 3,
            golesVisitante = 1
        };

        _partidoServicios.AgregarPartido(final, local, visitante, estadio);

        var campeon = _crucesServicio.ObtenerCampeonActual();

        Assert.AreEqual("Uruguay", campeon.nombre);
    }
    [TestMethod]
    public void ProcesarAvanceDelTorneo_SiNoHayPartidos_NoGeneraNada()
    {
        _crucesServicio.ProcesarAvanceDelTorneo();

        Assert.AreEqual(0, _partidoServicios.ObtenerPartidos().Count);
    }
}