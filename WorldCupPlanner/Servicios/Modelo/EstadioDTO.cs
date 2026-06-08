using System.ComponentModel.DataAnnotations;

namespace Servicios.Modelo;

public class EstadioDTO
{
    [Required(ErrorMessage = "El nombre del estadio es obligatorio")]                                                                                       
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La ciudad es obligatoria")]
    [StringLength(100, MinimumLength = 2)]
    public string Ciudad { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
    public string Descripcion { get; set; } = string.Empty;
    
    [Range(20000, 200000, ErrorMessage = "La capacidad debe estar entre 20000 y 200.000")]
    public int? CapacidadLocativa { get; set; }
}