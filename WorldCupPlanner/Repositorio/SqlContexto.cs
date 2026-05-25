using Dominio.Clases;
using Microsoft.EntityFrameworkCore;

namespace Repositorio;

public class SqlContexto: DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    public SqlContexto(DbContextOptions<SqlContexto> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}