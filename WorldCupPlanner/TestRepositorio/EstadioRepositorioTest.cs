using Repositorio;

namespace TestRepositorio;

[TestClass]
public class EstadioRepositorioTest
{
    private BaseDeDatosEnMemoria BDEnMemoria;
    private EstadioRepositorio _repositorioEstadio;

    [TestInitialize]
    public void Inicializar()
    {
        BDEnMemoria = new BaseDeDatosEnMemoria();
        _repositorioEstadio = new EstadioRepositorio(BDEnMemoria);
    }
    
}