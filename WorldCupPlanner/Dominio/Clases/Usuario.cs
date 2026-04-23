namespace Dominio.Clases;

public class Usuario
{
    private  string _nombre;
    private  string _apellido;
    private  string _email;
    private  DateTime _fechaNacimiento;
    private string _contraseña;
    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        _nombre = nombre;
        _apellido = apellido;
        _email = email;
        _fechaNacimiento = fechaNacimiento;
        _contraseña = contraseña;
    }
}