using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories
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

        public async Task<bool> DeleteFixedTermsByAccount(int accounId)
        {
            try
            {
                // Elimino las Transaction con el Id de la Account
                var fixedTermDeposits = await _context.FixedTermDeposits.Where(x => x.AccountId == accounId).ToListAsync();

                if (fixedTermDeposits != null)
                {
                    _context.FixedTermDeposits.RemoveRange(fixedTermDeposits);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}