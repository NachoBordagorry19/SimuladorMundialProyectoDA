using Dominio.Clases;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class SqlContexto: DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Estadio> Estadios { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    public DbSet<Auditoria> Auditorias { get; set; }

    public SqlContexto(DbContextOptions<SqlContexto> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
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

        modelBuilder.Entity<Partido>()
            .Ignore(p => p.Vencedor);

        modelBuilder.Entity<Partido>()
            .Property(p => p.Fase)
            .HasConversion<string>();

        modelBuilder.Entity<Partido>()
            .Property(p => p.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.Local)
            .WithMany()
            .HasForeignKey("LocalId")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.Visitante)
            .WithMany()
            .HasForeignKey("VisitanteId")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.Estadio)
            .WithMany()
            .HasForeignKey("EstadioId")
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.FechaHora)
                .IsRequired();

            entity.Property(a => a.Usuario)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValue(string.Empty);

            entity.Property(a => a.Accion)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValue(string.Empty);

            entity.Property(a => a.Detalle)
                .HasDefaultValue(string.Empty);
        });
    }
}