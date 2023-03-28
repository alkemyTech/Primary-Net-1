using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories;

    public class FixedTermDepositRepository : Repository<FixedTermDeposit>
    {
        public FixedTermDepositRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
