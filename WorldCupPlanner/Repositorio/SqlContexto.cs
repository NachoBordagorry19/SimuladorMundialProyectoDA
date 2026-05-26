using Dominio.Clases;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class SqlContexto: DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }

    public SqlContexto(DbContextOptions<SqlContexto> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Roles)
            .HasConversion(
                roles => string.Join(",", roles.Select(r => r.ToString())),
                value => value.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => Enum.Parse<Rol>(r))
                    .ToList()
            );
        
        modelBuilder.Entity<Equipo>()
            .Property(e => e.Confederacion)
            .HasConversion<string>();
    }
}