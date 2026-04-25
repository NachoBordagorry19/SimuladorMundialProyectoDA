namespace Dominio.Clases;

public class Grupo
{
    private string _nombre;

    public string Nombre
    {
        get => _nombre;
        set
        {
            if(EsVacio(value))
            {
                throw new ArgumentException("El nombre del grupo no puede ser vacio");
            }
            
            if(value.Length != 1 || value[0] < 'A' || value[0] > 'L')
            {
                throw new ArgumentException("El nombre del grupo debe ser una letra entre A y L");
            }
            _nombre = value;   
        }
    }
    public bool EsVacio(string textoATestear)
    {
        return string.IsNullOrEmpty(textoATestear);
    }
    
    public Grupo(string nombre)
    {
        Nombre = nombre;
    }
}