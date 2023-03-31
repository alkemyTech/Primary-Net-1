using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Services.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.Services.Repositories
{

    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<bool> Insert(Account entity)
        {
            // Si ya se encuentra un elemento con el mismo ID no intento agregarlo.
            if (_context.Accounts.Any(x => x.Id == entity.Id)) return false;

            return await base.Insert(entity);
        }

        public override async Task<bool> Update(Account entity)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(entity.Id);

                // Si no se encontró ninguna entidad con ese ID no intento actualizar.
                if (account is null) return false;

                account.Money = entity.Money;
                account.IsBlocked = entity.IsBlocked;

                _context.Accounts.Update(account);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public override async Task<bool> Delete(Account entity)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(entity.Id);

                // Si no se encontró ninguna entidad con ese ID no tiene sentido seguir.
                if (account is null) return false;

                _context.Accounts.Remove(account);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}