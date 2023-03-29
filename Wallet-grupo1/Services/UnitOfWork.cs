using Wallet_grupo1.DataAccess.Repositories;

namespace Wallet_grupo1.Services;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;

    public UserRepository UserRepo { get; private set; }
    public TransactionRepository TransactionRepo { get; private set; }
    public AccountRepository AccountRepo { get; }
    public CatalogueRepository CatalogueRepo { get;}
    public FixedTermDepositRepository FixedRepo {get;}
    public RoleRepository RoleRepo { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        //TODO: Inicializar cada uno de los repositorios concretos
        _context = context;
        UserRepo = new UserRepository(_context);
        TransactionRepo = new TransactionRepository(_context);
        AccountRepo = new AccountRepository(_context);
        CatalogueRepo = new CatalogueRepository(_context);
        FixedRepo = new FixedTermDepositRepository(_context);
        RoleRepo = new RoleRepository(_context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public Task<int> Complete()
    {
        return _context.SaveChangesAsync();
    }
}
