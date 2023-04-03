using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess.Repositories.Interfaces;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.Repositories{

    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Transaction>> TransactionsOfUser(int userId)
        {
            return await _context.Transactions.Where(x => x.Account.User.Id == userId).
                OrderByDescending(x => x.Date).ToListAsync();
        }

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
                    existingTransaction.Amount = transaction.Amount;
                    existingTransaction.Date = transaction.Date;
                    existingTransaction.Type= transaction.Type;
                }

                return true;

            }
            catch(Exception)
            {
                return false;
            }

        }

        public async Task<bool> RemoveReferencesToAccountId(int accounId)
        {
            try
            {
                var transactionsFromAccoundId = 
                    await _context.Transactions.Where(x => x.AccountId == accounId).ToListAsync();
                var transactionsToAccountId =
                    await _context.Transactions.Where(x => x.ToAccountId == accounId).ToListAsync();

                foreach (var t in transactionsFromAccoundId)
                {
                    t.AccountId = null;
                }

                foreach (var t in transactionsToAccountId)
                {
                    t.ToAccountId = null;
                }

                var allTransactionsToUpdate = transactionsFromAccoundId.Concat(transactionsToAccountId);
                
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
