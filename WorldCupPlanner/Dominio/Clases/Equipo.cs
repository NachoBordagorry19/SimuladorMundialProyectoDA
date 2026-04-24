namespace Dominio.Clases;

public class Equipo
{
    private string _nombre;
    private string _confederacion;
    private int _rankingFifa;

    public String Nombre
    {
        get => _nombre;
        set
        {
            if (EsVacio(value))
            {
                throw new ArgumentException("El nombre del equipo no puede ser vacio");
            }
            _nombre = value;
        }
    }
    
    public bool EsVacio(string textoATestear)
    {
        bool vacio = true;
        if (!String.IsNullOrEmpty(textoATestear))
        {
            vacio = false;
        }
        return vacio;
    }
    public Equipo(string nombre, string condeferacion, int rankingFifa)
    {
        Nombre = nombre;
        _confederacion = condeferacion;
        _rankingFifa = rankingFifa;
    }
}