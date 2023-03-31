using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services.Repositories.Interfaces;

namespace Wallet_grupo1.Services.Repositories{

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

    }
}
