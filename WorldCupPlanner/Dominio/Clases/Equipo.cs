namespace Dominio.Clases;

public class Equipo
{
    private string _nombre;
    private string _confederacion;
    private int _rankingFifa;
    public Equipo(string nombre, string condeferacion, int rankingFifa)
    {
        _nombre = nombre;
        _confederacion = condeferacion;
        _rankingFifa = rankingFifa;
    }
}