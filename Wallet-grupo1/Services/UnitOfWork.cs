using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DataAccess.Repositories;

namespace Wallet_grupo1.Services;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;

    public UserRepository UserRepo { get; private set; }
    public TransactionRepository TransactionRepo { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        //TODO: Inicializar cada uno de los repositorios concretos
        _context = context;
        UserRepo = new UserRepository(_context);
        TransactionRepo = new TransactionRepository(_context);
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
