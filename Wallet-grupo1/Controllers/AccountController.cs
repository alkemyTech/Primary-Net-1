using Microsoft.AspNetCore.Mvc;
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
    
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
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
    public IActionResult Insert([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.AccountRepo.Insert(account);
            uof.Complete();
        }  

        return CreatedAtAction(nameof(GetById), new { id = account.Id}, account);
    } 
    
    [HttpPost]
    public IActionResult Delete([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.AccountRepo.Delete(account);
            uof.Complete();
        }

        return Ok();
    }
    
    [HttpPut]
    public IActionResult Update([FromBody] Account account)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.AccountRepo.Update(account);
            uof.Complete();
        }

        return Ok();
    }
}