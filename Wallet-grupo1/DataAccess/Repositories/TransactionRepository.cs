using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories
{
    public class TransactionRepository : Repository<Transaction>
    {
        
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
       
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
                    existingTransaction.Type = transaction.Type;
                    existingTransaction.Account = transaction.Account;
                    existingTransaction.To_account = transaction.To_account;
                   

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
