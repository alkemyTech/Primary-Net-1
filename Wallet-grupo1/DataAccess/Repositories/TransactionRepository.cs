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
                
            }

        }

        public async Task<Transaction> GetByid(int id)
        {

        }

        public async void Insert(Transaction transaction)
        {
            try 
            {
                var existingTransaction = await _context.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefaultAsync();

                if (existingTransaction == null)
                {
                    _context.Set<Transaction>().Add(transaction);
                }
            }
            catch (Exception ex)
            {

            }
        }
        //public void Insert(Transaction transaction)
        //{
        //    _context.Transactions.Insert(transaction);
        //}

        public async void Delete(Transaction transaction)
        {
            try
            {
                var existingTransaction = await _context.Transactions.Where(x => x.Id == transaction.Id).FirstOrDefaultAsync();

                if (existingTransaction != null)
                {
                   _context.Set<Transaction>().Remove(transaction);
                }
                
            }
            catch (Exception ex)
            {

            }
        }
        
        
        //public void Delete(Transaction transaction)
        //{
        //    _context.Transactions.Delete(transaction);
        //}


        public override async void Update(Transaction transaction)
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

                    //Las claves foraneas se actualizan??
                }

            }
            catch(Exception ex)
            {

            }

        }

        //public void Update(Transaction transaction)
        //{ 
        //    _context.Transactions.Update(transaction);
        //}
        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
