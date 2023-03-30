using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories;

    public class FixedTermDepositRepository : Repository<FixedTermDeposit>
    {
        public FixedTermDepositRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public async Task<List<FixedTermDeposit>> FixedTermsOfUser(int userId)
        {
            return await _context.Transactions.Where(x => x.Account.User.Id == userId).
                OrderByDescending(x => x.Date).ToListAsync();
        }
    }
