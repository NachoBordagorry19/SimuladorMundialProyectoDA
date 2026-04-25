namespace Dominio.Clases;

public class Grupo
{
    private string _nombre;

    public string Nombre
    {
        get => _nombre;
        set => _nombre = value;
    }
    
    public Grupo(string nombre)
    {
        Nombre = nombre;
    }
}