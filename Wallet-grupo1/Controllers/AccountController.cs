using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [Authorize(Policy = "Admin")]
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
    
    [Authorize(Policy = "Admin")]
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
    
    [Authorize(Policy = "Admin")]
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

    [Authorize]
    [HttpPost("{id}")]
    public async  Task<IActionResult> Deposit([FromRoute] int id, [FromBody] decimal aumentoSaldo, [FromBody] string concept)
    {
        //Get token del header y validacion
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return Unauthorized("No se proporcionó un token de seguridad válido.");
        
        string jwtToken = authorizationHeader.Substring(7);
        
        // Extraigo el userid del token (es un claim)
        var userIdToken = GestorTokenJwt.ObtenerUserIdDeToken(jwtToken);
        if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

        Account? account;
        using (var uof = new UnitOfWork(_context))
        {
            account = uof.AccountRepo.GetById(id).Result;
        }

        if (account is null) return NotFound($"No se encontró ninguna cuenta con el número: {id}.");
        
        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (!account.validateUser(Int32.Parse(userIdToken))) 
            return Forbid("La cuenta no pertenece al usuario loggeado.");

        // Delego al gestor la logica del deposito.
        await new GestorOperaciones(_context).Depositar(account, aumentoSaldo, concept);

        return Ok($"Deposito realizado con éxito. Su nuevo saldo es: ${account.Money}.");
    }
}