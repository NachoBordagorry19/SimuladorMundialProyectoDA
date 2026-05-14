namespace Servicios.Modelo;

public class PosicionEquipoDTO
{
    public string EquipoNombre { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public EquipoDTO Equipo { get; set; } = new EquipoDTO();
    public int PartidosJugados { get; set; }
    public int Ganados { get; set; }
    public int Empatados { get; set; }
    public int Perdidos { get; set; }
    public int GolesAFavor { get; set; }
    public int GolesEnContra { get; set; }
    public int Diferencia { get; set; }
    public int Puntos { get; set; }
}