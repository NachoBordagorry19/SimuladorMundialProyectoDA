namespace Servicios.Modelo;

public class EstadioDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CapacidadLocativa { get; set; }
}