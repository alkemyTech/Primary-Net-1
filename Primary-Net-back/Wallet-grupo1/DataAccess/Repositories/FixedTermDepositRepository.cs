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

        public override async Task<bool> Update(FixedTermDeposit updatedDeposit)
        {
            try
            {
                var existingFixedTermDeposit = await _context.FixedTermDeposits.FindAsync(updatedDeposit.Id);

                // Si no se encontró ninguna entidad con ese ID no intento actualizar.
                if (existingFixedTermDeposit is null) return false;

                existingFixedTermDeposit.Amount = updatedDeposit.Amount;
                existingFixedTermDeposit.ClosingDate = updatedDeposit.ClosingDate;

                _context.FixedTermDeposits.Update(existingFixedTermDeposit);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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



        //usa este delete de referencia para crear el de este repositorio
        //         public override async Task<bool> Delete(Account entity)
        // {
        //     try
        //     {
        //         var account = await _context.Accounts.FindAsync(entity.Id);

        //         // Si no se encontró ninguna entidad con ese ID no tiene sentido seguir.
        //         if (account is null) return false;

        //         _context.Accounts.Remove(account);
        //     }
        //     catch (Exception)
        //     {
        //         return false;
        //     }

        //     return true;
        // }

        public override async Task<bool> Delete(FixedTermDeposit fixedTermDeposit)
        {
            try
            {


                // Si no se encontró ninguna entidad con ese ID no tiene sentido seguir.
                if (fixedTermDeposit is null) return false;

                _context.FixedTermDeposits.Remove(fixedTermDeposit);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}