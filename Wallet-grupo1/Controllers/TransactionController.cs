using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.AccessControl;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Transaction> transactions;
            using (var uof = new UnitOfWork(_context))
            {
                transactions = await uof.TransactionRepo.GetAll();
            }

            return Ok(transactions);
        }

        [HttpGet("{id: int}")]
        public async Task<IActionResult> GetById(int id)
        {
            Transaction? transaction;

            using(var uof = new UnitOfWork(_context))
            {
                transaction = await uof.TransactionRepo.GetById(id);
            }

            if (transaction == null) 
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]Transaction transaction) 
        {
            using(var uof = new UnitOfWork(_context))
            {
                await uof.TransactionRepo.Insert(transaction);
                await uof.Complete();
            }

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Transaction transaction)
        {
            using (var uof = new UnitOfWork(_context))
            {
                await uof.TransactionRepo.Delete(transaction);
                await uof.Complete();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]Transaction transaction)
        {
            using ( var uof = new UnitOfWork(_context))
            {
                await uof.TransactionRepo.Update(transaction);
                await uof.Complete();
            }

            return Ok();
        }
    }
}
