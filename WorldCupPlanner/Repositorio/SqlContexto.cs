using Dominio.Clases;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class SqlContexto: DbContext
{
    DbSet<Usuario> Usuarios { get; set; }
    DbSet<Equipo> Equipos { get; set; }
    DbSet<Estadio> Estadios { get; set; }
    DbSet<Partido> Partidos { get; set; }
    DbSet<Grupo> Grupos { get; set; }
    DbSet<Auditoria> Auditorias { get; set; }
    
    public SqlContexto(DbContextOptions<SqlContexto> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}