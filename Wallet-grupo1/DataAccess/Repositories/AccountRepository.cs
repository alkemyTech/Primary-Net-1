using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories;

public class AccountRepository : Repository<Account>
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }
    
}