using System.ComponentModel.DataAnnotations;

namespace Servicios.Modelo;

public class EstadioDTO
{
    [Required(ErrorMessage = "El nombre del estadio es obligatorio")]
    [StringLength(80, MinimumLength = 1, ErrorMessage = "El nombre no puede superar los 80 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria")]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "La ciudad no puede superar los 60 caracteres")]
    public string Ciudad { get; set; } = string.Empty;

    [StringLength(400, ErrorMessage = "La descripción no puede superar los 400 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La capacidad del estadio es obligatoria")]
    [Range(20000, 200000, ErrorMessage = "La capacidad debe estar entre 20.000 y 200.000")]
    public int? CapacidadLocativa { get; set; }
}