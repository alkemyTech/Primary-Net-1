using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Controllers
{
    [Authorize] // solo usuarios autenticados pueden acceder a este controlador
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll()
        {
            List<Transaction> transactions;
            using (var uof = new UnitOfWork(_context))
            {
                transactions = await uof.TransactionRepo.GetAll();
            }

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById([FromRoute] int id)
        {
            // obtener el ID del usuario autenticado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Transaction transaction;

            using (var uof = new UnitOfWork(_context))
            {
                transaction = await uof.TransactionRepo.GetById(id);
            }

            if (transaction == null)
            {
                return NotFound();
            }

            // verificar si la transacción pertenece al usuario autenticado
            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Insert(Transaction transaction)
        {
            // establecer el ID del usuario autenticado como propietario de la transacción
            transaction.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            using (var uof = new UnitOfWork(_context))
            {
                uof.TransactionRepo.Insert(transaction);
                await uof.CompleteAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // obtener el ID del usuario autenticado 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            using (var uof = new UnitOfWork(_context))
            {
                var transaction = await uof.TransactionRepo.GetById(id);
                if (transaction == null)
                {
                    return NotFound();
                }

                // verificar si la transacción pertenece al usuario autenticado
                if (transaction.UserId != userId)
                {
                    return Forbid();
                }

                uof.TransactionRepo.Delete(transaction);
                await uof.CompleteAsync();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            using (var uof = new UnitOfWork(_context))
            {
                var existingTransaction = await uof.TransactionRepo.GetById(id);
                if (existingTransaction == null)
                {
                    return NotFound();
                }

                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Description = transaction.Description;
                existingTransaction.Type = transaction.Type;

                uof.TransactionRepo.Update(existingTransaction);
                await uof.CompleteAsync();
            }

            return Ok();
        }
    }
}
