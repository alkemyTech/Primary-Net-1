using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services.Repositories.Interfaces;

namespace Wallet_grupo1.Services.Repositories
{
    
    public class FixedTermDepositRepository : Repository<FixedTermDeposit>, IFixedTermDepositRepository
    {
        public FixedTermDepositRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<FixedTermDeposit>> FixedTermsOfUser(int userId)
        {
            return await _context.FixedTermDeposits
                .Where(x => x.Account.User.Id == userId)
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync();
        }
    }
}