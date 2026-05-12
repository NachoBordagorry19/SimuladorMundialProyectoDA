using Dominio.Enums;
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

    [TestInitialize]
    public void Inicializar()
    {
        _baseDeDatos = new BaseDeDatosEnMemoria();
        _partidoRepositorio = new PartidoRepositorio(_baseDeDatos);
        _partidoServicios = new PartidoServicios(_partidoRepositorio);
        _crucesServicio = new CrucesSegundaFaseServicio(_partidoServicios);
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
                golesLocal = 3,
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
                golesLocal = 3,
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
                golesLocal = 2,
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
                golesLocal = 2,
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
                golesLocal = 1,
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
}