using Dominio.Clases;
using Repositorio.Interfaces;

namespace Repositorio;

public class AuditoriaRepositorio : IAuditoriaRepositorio
{
    private readonly List<Auditoria> _registros;

    public AuditoriaRepositorio()
    {
        _registros = new List<Auditoria>();
    }

    public void AgregarRegistro(Auditoria registro)
    {
        _registros.Add(registro);
    }

    public List<Auditoria> ObtenerTodosLosRegistros()
    {
        return new List<Auditoria>(_registros);
    }
}

