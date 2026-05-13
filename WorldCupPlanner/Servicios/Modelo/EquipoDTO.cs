using Dominio.Enums;

namespace Servicios.Modelo;

public class EquipoDTO
{
    public string nombre { get; set; } = string.Empty;
    public Confederacion confederacion { get; set; }
    public int rankingFifa { get; set; }
}