using Wallet_grupo1.DataAccess;

namespace Wallet_grupo1.Services;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        //TODO: Inicializar cada uno de los repositorios concretos
        _context = context;
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public int complete()
    {
        return _context.SaveChanges();
    }
}
