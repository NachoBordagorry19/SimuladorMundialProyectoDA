using System.ComponentModel.DataAnnotations;
using Dominio.Enums;

namespace Servicios.Modelo;

public class EquipoDTO
{
    [Required(ErrorMessage = "El nombre del equipo es obligatorio")]                                                                                        
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La confederación es obligatoria")]
    public Confederacion confederacion { get; set; }
    
    [Range(1, 2500, ErrorMessage = "El ranking FIFA debe estar entre 1 y 2500")]
    public int rankingFifa { get; set; }
    public string banderaBase64 { get; set; } = string.Empty;
}