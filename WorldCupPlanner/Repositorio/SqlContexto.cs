using Dominio.Clases;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class SqlContexto: DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Estadio> Estadios { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Auditoria> Auditorias { get; set; }
    
    public SqlContexto(DbContextOptions<SqlContexto> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}