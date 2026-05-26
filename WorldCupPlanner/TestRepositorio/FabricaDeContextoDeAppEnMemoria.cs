using Microsoft.EntityFrameworkCore;
using Repositorio;

namespace TestRepositorio;

public class FabricaDeContextoDeAppEnMemoria
{
    public SqlContexto CrearDbContexto()
    {
        var opcionesBuilder = new DbContextOptionsBuilder<SqlContexto>();
        opcionesBuilder.UseInMemoryDatabase("TestingDB_");
        return new SqlContexto(opcionesBuilder.Options);
    }
}