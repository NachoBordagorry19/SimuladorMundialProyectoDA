namespace Dominio.Clases;
using Dominio.Enums;

public class Equipo
{
    public string Nombre { get; }
    public Confederacion Confederacion { get; }
    public int RankingFifa { get; }

    public Equipo(string nombre, Confederacion confederacion, int rankingFifa)
    {
        Nombre = nombre;
        Confederacion = confederacion;
        RankingFifa = rankingFifa;
    }
}