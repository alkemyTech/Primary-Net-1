using Wallet_grupo1.DataAccess.Repositories;

namespace Wallet_grupo1.Services;

public interface IUnitOfWork : IDisposable
{
    public UserRepository UserRepo { get; }
    public TransactionRepository TransactionRepo { get; }
    public AccountRepository AccountRepo { get; }
    public FixedTermDepositRepository FixedRepo { get; }
    public CatalogueRepository CatalogueRepo { get; }
    public RoleRepository RoleRepo { get; }
    
    Task<int> Complete();
}