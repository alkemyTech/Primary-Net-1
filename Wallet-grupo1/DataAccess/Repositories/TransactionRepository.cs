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

        public override async List<Transaction> GetAll()
        {
            try
            {
                return await 
            }
            catch { Exception ex}
            {

            }
        }
        public void Insert(Transaction transaction)
        {
            _context.Transactions.Insert(transaction);
        }

        public void Delete(Transaction transaction)
        {
            _context.Transactions.Delete(transaction);
        }

        public void Update(Transaction transaction)
        { 
            _context.Transactions.Update(transaction);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
