using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("/api/account")]
public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    public AccountController(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _unitOfWorkService.AccountRepo.GetAll();
        
        if (accounts.Count < 1)
        {
            //TODO refactor con manejo de errores y respuestas vacias
            return NotFound();
        }
        return Ok(accounts);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var account = await _unitOfWorkService.AccountRepo.GetById(id);

        if (account == null)
        {
            //TODO refactor con manejo de errores y respuestas vacias
            return NotFound();
        }
        
        return Ok(account);
    }

    
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Account account)
    {
        await _unitOfWorkService.AccountRepo.Insert(account);
        await _unitOfWorkService.Complete();

        return CreatedAtAction(nameof(GetById), new { id = account.Id}, account);
    } 
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var account = await _unitOfWorkService.AccountRepo.GetById(id);

        if (account is null) return NotFound($"No se encontro ninguna cuenta con el id: {id}.");

        var result = await _unitOfWorkService.AccountRepo.Delete(account);

        if (!result)
            return StatusCode(500, $"No se pudo eliminar la cuenta con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Account account)
    {

        var result = await _unitOfWorkService.AccountRepo.Update(account);
            
        if (!result)
            return StatusCode(500, $"No se pudo actualizar la account con id: {account.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción."); 
                                       
        await _unitOfWorkService.Complete();
        
        return Ok();
    }

    [Authorize]
    [HttpPost("deposit/{id}")]
    public async Task<IActionResult> Deposit([FromRoute] int id, [FromBody] DepositDto dto)
    {
        //Get token del header y validacion
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return Unauthorized("No se proporcionó un token de seguridad válido.");
        
        string jwtToken = authorizationHeader.Substring(7);
        
        // Extraigo el userid del token (es un claim)
        var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
        if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

        Account? account;
        
        account = _unitOfWorkService.AccountRepo.GetById(id).Result;

        if (account is null) return NotFound($"No se encontró ninguna cuenta con el número: {id}.");
        
        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (account.UserId != int.Parse(userIdToken))
            return Forbid("La cuenta no pertenece al usuario loggeado.");

        // Delego al gestor la logica del deposito.
        await new GestorOperaciones(_unitOfWorkService).Deposit(account, dto.AumentoSaldo, dto.Concept);

        return Ok($"Deposito realizado con éxito. Su nuevo saldo es: ${account.Money}.");
    }

    [Authorize]
    [HttpPost("transferencia/{id}")]
    public async Task<IActionResult> Transferencia([FromRoute] int id, [FromBody] TransferenciaDto dto)
    {
        //Get token del header y validacion
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return Unauthorized("No se proporcionó un token de seguridad válido.");

        string jwtToken = authorizationHeader.Substring(7);

        // Extraigo el userid del token (es un claim)
        var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
        if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

        // account = Quien envia el dinero, toAccount = Quien recibe el dinero
        var account = _unitOfWorkService.AccountRepo.GetById(id).Result;
        var toAccount = _unitOfWorkService.AccountRepo.GetById(dto.IdReceptor).Result;
        
        //TODO manejo de errores y respuestas vacias
        if (account is null) return NotFound($"No se encontró ninguna cuenta con el número: {id}.");
        if (toAccount is null) return NotFound($"No se encontró ninguna cuenta con el número: {dto.IdReceptor}.");

        if (account.Money < dto.MontoTransferido) return StatusCode(500, $"El monto a enviar es mayor al que contiene en la cuenta.");

        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (account.UserId != int.Parse(userIdToken))
            return Forbid("La cuenta no pertenece al usuario loggeado.");

        // Delego al gestor la logica del transferir el dinero.
        await new GestorOperaciones(_unitOfWorkService).Transfer(account, toAccount, dto.MontoTransferido, dto.Concept);

        return Ok($"Transferencia realizada con éxito.");
    }
}