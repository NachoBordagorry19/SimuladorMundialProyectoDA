using Dominio.Enums;

namespace Servicios.Modelo;

public class UsuarioDTO
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Contraseña { get; set; } = "";

    public List<Rol> Roles { get; set; } = new();
}