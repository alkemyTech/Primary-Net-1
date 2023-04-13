using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{
    /// <summary>
    ///     Repositorio transaction
    /// </summary>
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        /// <summary>
        ///     Constructor transaction
        /// </summary>
        /// <param name="context"></param>
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        ///     Se obtienen transacciones del usuario por id de su cuenta
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> TransactionsOfUser(int accountId)
        {
            return await _context.Transactions
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        /// <summary>
        ///     Funcion para eliminar una transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(Transaction transaction)
        {
            try
            {
                var existingTransaction = await _context.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefaultAsync();
                if (existingTransaction != null)
                {
                   _context.Set<Transaction>().Remove(transaction);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Funcion para actualizar Transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        
        public override async Task<bool> Update(Transaction transaction)
        {
            try
            {
                // Verifico la existencia de la transaccion a actualizar y luego la actualizo
                var existingTransaction = await _context.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefaultAsync();

                if (existingTransaction == null)
                {
                    _context.Set<Transaction>().Add(transaction);
                }
                else
                {
                    existingTransaction.Concept= transaction.Concept;
                }

                return true;

            }
            catch(Exception)
            {
                return false;
            }

        }

        /// <summary>
        ///     Se eliminan las  transacciones con referencia a una account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveReferencesToAccountId(int accountId)
        {
            try
            {
                var transactionsFromAccountId = 
                    await _context.Transactions.Where(x => x.AccountId == accountId).ToListAsync();
                var transactionsToAccountId =
                    await _context.Transactions.Where(x => x.ToAccountId == accountId).ToListAsync();

                foreach (var t in transactionsFromAccountId)
                {
                    t.AccountId = null;
                }

                foreach (var t in transactionsToAccountId)
                {
                    t.ToAccountId = null;
                }

                var allTransactionsToUpdate = transactionsFromAccountId.Concat(transactionsToAccountId);
                
                _context.Transactions.UpdateRange(allTransactionsToUpdate);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
