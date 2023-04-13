using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
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

    /// <summary>
    /// Obtener el listado de todas las Account del sistema. Solo usuarios administradores pueden acceder.
    /// </summary>
    /// <returns>Listado de todas las Account del sistema.</returns>
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _unitOfWorkService.AccountRepo.GetAll();

        // Pagina default
        int pageToShow = 1;
        // Recupera de la request el parametro page, si no esta esa key se va a mostrar la default(1)
        if (Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
        // recupera la url sin los query params para poder usarla en la paginacion 
        var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString();

        // Pagina los resultados
        var paginatedAccounts = PaginateHelper.Paginate(accounts, pageToShow, url);

        if (accounts.Count < 1)
        {
            //TODO refactor con manejo de errores y respuestas vacias
            return NotFound();
        }
        return Ok(paginatedAccounts);
    }

    /// <summary>
    /// Obtener una Account a partir del ID especificado. Solo los administradores tienen acceso.
    /// </summary>
    /// <param name="id">ID del rol que se quiere recuperar.</param>
    /// <returns>El estado de la Account con el ID especificado.</returns>
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

    /// <summary>
    /// Insertar una Account en la base de datos con los datos pasados en el Body.
    /// </summary>
    /// <param name="accountDTO"></param>
    /// <returns>El resultado de la creación e inserción de la entidad y su estado.</returns>
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Insert()
    {
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null)
            return ResponseFactory.CreateErrorResponse(401,
            "No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return ResponseFactory.CreateErrorResponse(401,
                "No se proporcionó un token de seguridad válido.");

        string jwtToken = authorizationHeader.Substring(7);

        // Extraigo el userid del token (es un claim)
        var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
        if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

        // Busco el Id de la Account del User
        var userAccount = await _unitOfWorkService.AccountRepo.FindByUserId(Int32.Parse(userIdToken));
        if (userAccount is not null)
            return ResponseFactory.CreateErrorResponse(405,
                $"Usted ya posee una cuenta asociada, la cual responde al ID: {userAccount.Value.UserId}.");

        var user = await _unitOfWorkService.UserRepo.GetById(Int32.Parse(userIdToken));

        var theNewAccount = new Account
        {
            IsBlocked = false,
            Money = 0,
            CreationDate = DateTime.Now,
            UserId = Int32.Parse(userIdToken),
            User = user
        };

        await _unitOfWorkService.AccountRepo.Insert(theNewAccount);
        await _unitOfWorkService.Complete();
        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(201, theNewAccount);
    }

    /// <summary>
    /// Eliminar la Account de la base de datos cuyo ID corresponda con el especificado.
    /// </summary>
    /// <param name="id">ID de la Account que se desea eliminar.</param>
    /// <returns>Resultado de la transacción de eliminación</returns>
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var account = await _unitOfWorkService.AccountRepo.GetById(id);
        if (account is null) return NotFound($"No se encontro ninguna cuenta con el id: {id}.");

        // Elimino las Transaccion con Id la Account
        var deletedTransaccions = await _unitOfWorkService.TransactionRepo.RemoveReferencesToAccountId(account.Id);
        if (!deletedTransaccions)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar la Transaccion del user con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        // Elimino los FixedTermDeposit con la Id de la Account
        var deletedFixdTermDeposit = await _unitOfWorkService.FixedRepo.DeleteFixedTermsByAccount(account.Id);
        if (!deletedFixdTermDeposit)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar FixedTerm del user con id: {id}" +
                                                            $" porque no existe o porque no se pudo completar la transacción.");

        // Elimino la Account con el Id del User
        var deletedAccount = await _unitOfWorkService.AccountRepo.Delete(account);
        if (!deletedAccount)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar la account del user con id: {id}" +
                                                            $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200,
            $"Se eliminó la cuenta con ID: {account.Id}, sus plazos fijos y todas sus referencias en transacciones.");
    }

    /// <summary>
    /// Actualizar el estado de una Account con los datos pasados en el body.
    /// </summary>
    /// <param name="account">Información de la Account a actualizar.</param>
    /// <returns>Resultado de la transacción de actualización.</returns>
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

    /// <summary>
    /// Operacion de Deposito en una Account
    /// </summary>
    /// <param name="id">ID de la cuenta a depositar.</param>
    /// <param name="dto">Cuenta receptora.</param>
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

    /// <summary>
    /// Operacion de Transferencia en una Account
    /// </summary>
    /// <param name="id">ID de la cuenta que Transfiere.</param>
    /// <param name="dto">Cuenta receptora.</param>
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