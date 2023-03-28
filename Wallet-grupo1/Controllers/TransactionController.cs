using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Wallet_grupo1.DataAccess;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.AccessControl;

using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{

    [Authorize] // solo usuarios autenticados pueden acceder a este controlador
    [Route("Transaction")]

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
            Transaction? transaction;
            using (var uof = new UnitOfWork(_context))
            {
                transaction = await uof.TransactionRepo.GetById(id);
            }

            if (transaction is null) return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Insert(Transaction transaction)
        {
            using (var uof = new UnitOfWork(_context))
            {
                await uof.TransactionRepo.Insert(transaction);
                await uof.Complete();
            }  

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id}, transaction);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            using (var uof = new UnitOfWork(_context))
            {
                var transaction = await uof.TransactionRepo.GetById(id);

                if (transaction is null) return NotFound($"No se encontro ninguna transacción con el id: {id}.");
                
                var result = await uof.TransactionRepo.Delete(transaction);

                if (!result)
                    return StatusCode(500, $"No se pudo eliminar la transacción con id: {id}" +
                                           $" porque no existe o porque no se pudo completar la transacción.");
                                       
                await uof.Complete();
            }

            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {
            using (var uof = new UnitOfWork(_context))
            {
                var result = await uof.TransactionRepo.Update(transaction);
            
                if (!result)
                    return StatusCode(500, $"No se pudo actualizar la transaccion con id: {transaction.Id}" +
                                           $" porque no existe o porque no se pudo completar la transacción."); 
                                       
                await uof.Complete();
            }

            return Ok();
        }
    }
}
