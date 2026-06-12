using Servicios.Modelo;

namespace UI.Estado;

public class FixtureEstado
{
    public ResultadoFixture? ResultadoFixture { get; set; }

    public CuadroSegundaFaseDTO? CuadroSegundaFase { get; set; }

    public string? GrupoSeleccionado { get; set; }

    public EquipoDTO? Campeon { get; set; }

    public string NombreMotorSimulacion { get; set; } = string.Empty;
}