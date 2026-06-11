using Dominio.Enums;

namespace Servicios.Modelo;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Contraseña { get; set; } = "";

    public List<Rol> Roles { get; set; } = new();
}