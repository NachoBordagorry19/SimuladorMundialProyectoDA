using Dominio.Enums;

namespace Dominio.Clases;

public class Incidencia
{
    public int _id { get; set; }
    public int _idPartido { get; set; }
    public int _idEquipo { get; set; }
    public TipoIncidencia _tipoIncidencia { get; set; }

    public Incidencia(int idPartido, int idEquipo, TipoIncidencia tipo)
    {
        _idPartido = idPartido;
        _idEquipo = idEquipo;
        _tipoIncidencia = tipo;
    }

}