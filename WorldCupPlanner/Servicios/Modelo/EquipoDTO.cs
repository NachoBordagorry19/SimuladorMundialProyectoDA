using System.ComponentModel.DataAnnotations;
using Dominio.Enums;

namespace Servicios.Modelo;

public class EquipoDTO
{
    [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "El nombre no puede superar los 60 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confederación es obligatoria")]
    public Confederacion Confederacion { get; set; }

    [Range(300, 2500, ErrorMessage = "El ranking FIFA debe estar entre 300 y 2500")]
    public int RankingFifa { get; set; }
    public string BanderaBase64 { get; set; } = string.Empty;
}