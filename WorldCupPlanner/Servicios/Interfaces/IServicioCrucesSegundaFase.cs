using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioCrucesSegundaFase
{
    List<PosicionEquipoDTO> ObtenerRankingGrupo(string grupo, int semillaCrucesFase);

    ClasificadosDTO ObtenerClasificados(int semillaCrucesFase);

    List<CruceDTO> GenerarCrucesFase(ClasificadosDTO clasificados, int semillaCrucesFase);

    List<CruceDTO> GenerarOctavosDeFinal(List<CruceDTO> crucesDieciseisavos);

    List<CruceDTO> GenerarCuartosDeFinal(List<CruceDTO> octavos);

    List<CruceDTO> GenerarSemifinales(List<CruceDTO> cuartos);

    List<CruceDTO> GenerarTercerPuestoYFinal(List<CruceDTO> semifinales);

    CuadroSegundaFaseDTO GenerarCuadroSegundaFase(int semillaCrucesFase);
    CuadroSegundaFaseDTO ObtenerCuadroActual();
    void ProcesarAvanceDelTorneo();

    EquipoDTO? ObtenerCampeonActual();
    EquipoDTO ObtenerCampeon(PartidoDTO partidoFinal);
}