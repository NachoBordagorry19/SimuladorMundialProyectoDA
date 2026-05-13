using Servicios.Modelo;

namespace Servicios.Interfaces;

public interface IServicioCrucesSegundaFase
{
    ClasificadosDTO ObtenerClasificados(int semillaCrucesFase);

    List<CruceDTO> GenerarCrucesFase(ClasificadosDTO clasificados, int semillaCrucesFase);

    List<CruceDTO> GenerarOctavosDeFinal(List<CruceDTO> crucesDieciseisavos);

    List<CruceDTO> GenerarCuartosDeFinal(List<CruceDTO> octavos);

    List<CruceDTO> GenerarSemifinales(List<CruceDTO> cuartos);

    List<CruceDTO> GenerarTercerPuestoYFinal(List<CruceDTO> semifinales);

    EquipoDTO ObtenerCampeon(PartidoDTO partidoFinal);
}