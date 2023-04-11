using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories
{
    /// <summary>
    /// Repositorio concreto asociado a los plazos fijos
    /// </summary>
    public class FixedTermDepositRepository : Repository<FixedTermDeposit>, IFixedTermDepositRepository
    {
        
        public FixedTermDepositRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        
        /// <summary>
        /// Actualizacion de campos del plazo fijo en funcion de request HTTP
        /// </summary>
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

        /// <summary>
        /// Busqueda y listado de plazo fijos para usuario en funcion de request HTTP
        /// </summary>
        public async Task<List<FixedTermDeposit>> FixedTermsOfUser(int userId)
        {
            return await _context.FixedTermDeposits
                .Where(x => x.Account.User.Id == userId)
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync();
        }

        /// <summary>
        /// Busqueda y listado de plazo fijos para usuario en funcion de request HTTP
        /// </summary>
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

        /// <summary>
        /// Eliminación de plazo fijo por admin en funcion de request HTTP
        /// </summary>
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