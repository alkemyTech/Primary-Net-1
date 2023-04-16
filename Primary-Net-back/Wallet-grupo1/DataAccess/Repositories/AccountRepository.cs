using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories
{
    /// <summary>
    /// Repositorio para manipular entidades de cuentas
    /// </summary>
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        /// <summary>
        /// Constructor base
        /// </summary>
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Redefinicion de update cuentas segun el DTO correspondiente
        /// </summary>
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

        /// <summary>
        /// Redefinicion de eliminacion de cuentas
        /// </summary>
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

        /// <summary>
        /// Localizacion de cuenta de usuario por ID
        /// </summary>
        public async Task<ActionResult<Account>> FindByUserId(int userId)
        {
            // Busca la Account del user por Id
            Account? account = await _context.Accounts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            //if (account is null) throw new Exception();

            return account;

        
        }
    }
}