using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Account> accounts;
        
        using (var uof = new UnitOfWork(_context))
        {
            accounts = await uof.AccountRepo.GetAll();
        }

        return Ok(accounts);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Account? account;

        using (var uof = new UnitOfWork(_context))
        {
            account = await uof.AccountRepo.GetById(id);
        }

        if (account is null) return NotFound();
        
        return Ok(account);
    }
    
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            await uof.AccountRepo.Insert(account);
            await uof.Complete();
        }  

        return CreatedAtAction(nameof(GetById), new { id = account.Id}, account);
    } 
    
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            var result = await uof.AccountRepo.Delete(account);

            if (!result)
                return StatusCode(500, $"No se pudo eliminar la account con id: {account.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");
                                       
            await uof.Complete();
        }

        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            var result = await uof.AccountRepo.Update(account);
            
            if (!result)
                return StatusCode(500, $"No se pudo actualizar la account con id: {account.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción."); 
                                       
            await uof.Complete();
        }

        return Ok();
    }
}