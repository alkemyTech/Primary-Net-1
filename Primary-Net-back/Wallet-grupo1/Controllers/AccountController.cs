using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

/// <summary>
/// Controlador de entidad cuentas
/// </summary>
[Route("/api/account")]
public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    /// <summary>
    /// Constructor base para el controlador de cuentas con la unidad CRUD de trabajo
    /// </summary>
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
            return ResponseFactory.CreateErrorResponse(500, "Hubo un error intentando " +
                                                            $"obtener los usuarios del sistema");
        }
        return ResponseFactory.CreateSuccessfullyResponse(200, paginatedAccounts);
    }

    /// <summary>
    /// Obtener una Account a partir del ID especificado. Solo los administradores tienen acceso.
    /// </summary>
    /// <param name="id">ID del rol que se quiere recuperar.</param>
    /// <returns>El estado de la Account con el ID especificado.</returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _unitOfWorkService.AccountRepo.GetById(id);

        if (account == null)
        {
            return ResponseFactory.CreateErrorResponse(404, "No se pudo localizar" +
                                                            $" a la cuenta de ID: {id} en el sistema");
        }
        return ResponseFactory.CreateSuccessfullyResponse(200, account);
    }

    /// <summary>
    /// Crear una Account en el sistema para un usuario registrado.
    /// </summary>
    /// <param name="userId">Usuario que quiere crear la Account. El ID se autogenerará en la BD.</param>
    /// <returns>El resultado de la creación e inserción de la entidad y su estado.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNewAccount()
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

        var theNewAccount = new Account
        {
            IsBlocked = false,
            Money = 0,
            CreationDate = DateTime.Now,
            UserId = Int32.Parse(userIdToken)
        };
        
        await _unitOfWorkService.AccountRepo.Insert(theNewAccount);
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
        if (account is null) 
            return ResponseFactory.CreateErrorResponse(404, "No se pudo localizar" +
            $" la cuenta con ID: {id} en el sistema");

        // Elimino las Transaccion con Id la Account
        var deletedTransactions = await _unitOfWorkService.TransactionRepo.RemoveReferencesToAccountId(account.Id);
        if (!deletedTransactions)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar la Transaccion del user con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        // Elimino los FixedTermDeposit con la Id de la Account
        var deletedFixedTermDeposit = await _unitOfWorkService.FixedRepo.DeleteFixedTermsByAccount(account.Id);
        if (!deletedFixedTermDeposit)
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
    /// Actualizar el estado de una Account con los datos pasados en el body. Requiere permisos de administrador.
    /// </summary>
    /// <param name="id">ID de la Account a actualizar.</param>
    /// <param name="accountDto">Información de la Account a actualizar.</param>
    /// <returns>Resultado de la transacción de actualización.</returns>
    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AccountDto accountDto)
    {
        if (accountDto.Money < 0)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar la cuenta con ID: {id}. Revisar" +
                                                            $"campos insertados.");
        
        //Instancio la clase en base al DTO con los campos actualizados
        var theNewAccountData = new Account(id, accountDto);
        var result = await _unitOfWorkService.AccountRepo.Update(theNewAccountData);

        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar la cuenta con ID: {id}. Revisar" +
                                                            $"campos insertados.");
        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200, $"La cuenta con ID: {id} se actualizó con éxito.");
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

        if (authorizationHeader is null) 
            return ResponseFactory.CreateErrorResponse(401,
                $"No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return ResponseFactory.CreateErrorResponse(401,
                $"No se proporcionó un token de seguridad válido.");

        string jwtToken = authorizationHeader.Substring(7);

        // Extraigo el userid del token (es un claim)
        var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
        if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

        Account? account;

        account = _unitOfWorkService.AccountRepo.GetById(id).Result;

        if (account is null) 
            return ResponseFactory.CreateErrorResponse(403,$"No se encontró ninguna cuenta con el número: {id}.");

        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (account.UserId != int.Parse(userIdToken))
            return ResponseFactory.CreateErrorResponse(403,"La cuenta no pertenece al usuario logueado.");

        // Delego al gestor la logica del deposito.
        await new GestorOperaciones(_unitOfWorkService).Deposit(account, dto.AumentoSaldo, dto.Concept);

        return ResponseFactory.CreateSuccessfullyResponse(200,$"Deposito realizado con éxito. Su nuevo saldo es: ${account.Money}.");
    }

    /// <summary>
    /// Operacion de Transferencia en una Account
    /// </summary>
    /// <param name="id">ID de la cuenta que Transfiere.</param>
    /// <param name="dto">Cuenta receptora.</param>
    [Authorize]
    [HttpPost("transfer/{id}")]
    public async Task<IActionResult> Transfer([FromRoute] int id, [FromBody] TransferenciaDto dto)
    {
        //Get token del header y validacion
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

        // account = Quien envia el dinero, toAccount = Quien recibe el dinero
        var account = _unitOfWorkService.AccountRepo.GetById(id).Result;
        
        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (account.UserId != int.Parse(userIdToken))
            return ResponseFactory.CreateErrorResponse(403,"La cuenta no pertenece al usuario loggeado.");
        
        var toAccount = _unitOfWorkService.AccountRepo.GetById(dto.IdReceptor).Result;
        
        if (account is null) 
            return ResponseFactory.CreateErrorResponse(404,
                "No se encontro una cuenta asociada a su usuario.");
        if (toAccount is null) 
            return ResponseFactory.CreateErrorResponse(404,
                $"No se encontró ninguna cuenta con el número: {dto.IdReceptor}.");

        if (account.Money < dto.MontoTransferido) 
            return ResponseFactory.CreateErrorResponse(500, "El monto a enviar es mayor al que contiene en la cuenta.");

        // Delego al gestor la logica del transferir el dinero.
        await new GestorOperaciones(_unitOfWorkService).Transfer(account, toAccount, dto.MontoTransferido, dto.Concept);

        return ResponseFactory.CreateSuccessfullyResponse(200,"Transferencia realizada con éxito.");
    }
}