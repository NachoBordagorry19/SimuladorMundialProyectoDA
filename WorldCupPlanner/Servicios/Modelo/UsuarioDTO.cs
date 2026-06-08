using System.ComponentModel.DataAnnotations;
using Dominio.Enums;

namespace Servicios.Modelo;

public class UsuarioDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]                                                                                                   
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es obligatorio")]                                                                                                 
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
    public string Apellido { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }
    
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un carácter especial")]
    public string Contraseña { get; set; } = "";

    [MinLength(1, ErrorMessage = "El usuario debe tener al menos un rol")]
    public List<Rol> Roles { get; set; } = new();
}