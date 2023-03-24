using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories
{
    public class UserRepository : Repository<User>
    {
        
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            
        }

    }
}
