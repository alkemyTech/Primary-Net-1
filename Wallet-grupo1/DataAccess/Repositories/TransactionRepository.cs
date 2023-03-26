using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.DataAccess.Repositories
{
    public class TransactionRepository : Repository<Transaction>
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Transaction>> GetAll()
        {
            try
            {
                return await _context.Set<Transaction>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante GetAll: {ex.Message}");
                throw;
            }

        }

        public async Task<Transaction> GetByid(int id)
        {

            try
            {
                return await _context.Set<Transaction>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errir con ID {id}: {ex.Message}");
                throw;
            }

        }

        public override async Task<bool> Insert(Transaction transaction)
        {
            try 
            {
                var existingTransaction = await _context.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefaultAsync();

                if (existingTransaction == null)
                {
                    _context.Set<Transaction>().Add(transaction);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
            catch (Exception ex)
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
                    existingTransaction.Account_id = transaction.Account_id;
                    existingTransaction.User_id = transaction.User_id;
                    existingTransaction.To_account_id = transaction.To_account_id;
                }

                return true;

            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
