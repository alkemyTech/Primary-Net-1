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

/// <summary>
/// Controlador que provee la funcionalidades asociadas a los plazos fijos/inversiones con la wallet.
/// </summary>
[Route("api/fixedTerm")]
public class FixedController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    /// <summary>
    /// Constructor base con su unidad de trabajo asociada las operaciones CRUD.
    /// </summary>
    public FixedController(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }
    
    /// <summary>
    /// Endpoint que lista todos los plazos fijos en el sistema.
    /// Requiere permisos de administrador.
    /// </summary>
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Carga todos los Fixed de la base de datos utilizando el repositorio de Fixed
        var Fixed = await _unitOfWorkService.FixedRepo.GetAll();

        // Paginar el resultado
        int pageToShow = 1;
        if (Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
        var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString();

        var paginatedFixed = PaginateHelper.Paginate(Fixed, pageToShow, url);

        if (Fixed == null)
        {
            return StatusCode(204, "No se encontraron FixedTermDeposits");
        }
        // Retorna un código 200 (OK)
        return Ok(paginatedFixed);
    }

    /// <summary>
    /// Endpoint que lista los detalles de un plazo fijo segun su código identificador.
    /// </summary>
    /// <param name="id">ID del plazo fijo que se desea consultar.</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // Obtiene el Id del fidex especificado utilizando el repositorio de Fixed.
        var Fixed = await _unitOfWorkService.FixedRepo.GetById(id);
        ///Si no se encuentra un Fixed con el Id especificado, devulve un código 404 
        if (Fixed is null) return NotFound();
        // Si se encuentra el Fixed, retorna un código 200 
        return Ok(Fixed);
    }
    
    /// <summary>
    /// Endpoint que inserta los detalles de un plazo fijo a partir del input de un usuario registrado.
    /// </summary>
    /// <param name="newFixedTermDto">El constructor del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize]
    [HttpPost("deposit")]
    public async Task<IActionResult> CreateFixedTerm([FromBody] FixedTermDepositRequestDto newFixedTermDto)
    {
        //Usamos el DTO para construir la entidad con los datos necesarios
        var theNewFixedTerm = new FixedTermDeposit(newFixedTermDto.NewFixedTermDeposit);
        
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

        account = _unitOfWorkService.AccountRepo.GetById(theNewFixedTerm.AccountId).Result;

        if (account is null) return NotFound($"No se encontró ninguna cuenta con el número: {theNewFixedTerm.AccountId}.");

        // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
        if (account.UserId != int.Parse(userIdToken))
            return Forbid("La cuenta no pertenece al usuario loggeado.");
        
        // Delego al gestor la logica de creacion del plazo fijo.
        await new GestorOperaciones(_unitOfWorkService).CreateFixedTerm(account, theNewFixedTerm, newFixedTermDto.InterestRate);

        return ResponseFactory.CreateSuccessfullyResponse(201, newFixedTermDto);
    }
    
    /// <summary>
    /// Endpoint que elimina la fila de un plazo fijo a partir del código identificador.
    /// </summary>
    /// <param name="id">El identificador del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFixedTermDeposit([FromRoute] int id)
    {
        // Obtiene el Id del FixedTerm especificado utilizando el repositorio de FixedTerms.
        var fixedTermDeposit = await _unitOfWorkService.FixedRepo.GetById(id);

        if (fixedTermDeposit is null) return ResponseFactory.CreateErrorResponse(404, $"No se encontró ningún plazo fijo con el id: {id}.");

        var result = await _unitOfWorkService.FixedRepo.Delete(fixedTermDeposit);
        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar el plazo fijo con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();
        
        return ResponseFactory.CreateSuccessfullyResponse(200, "El plazo fijo se eliminó con éxito.");
    }


    /// <summary>
    /// Endpoint que actualiza un plazo fijo segun su código identificador. Requiere permisos de administrador
    /// </summary>
    /// <param name="Fixed">El constructor del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] FixedTermDepositDto dto)
    {
        var result = await _unitOfWorkService.FixedRepo.Update(new FixedTermDeposit(id, dto));
        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar el plazo fijo con ID: {id}.");

        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200, $"El plazo fijo con ID: {id} se actualizó con éxito.");
    }

    
    /// <summary>
    /// Endpoint que lista los plazos segun el código identificador del usuario consumidor.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [HttpGet("{userId}")]
    public async Task<List<FixedTermDeposit>> FixedTermsOfUser([FromBody] int userId)
    {
        var resultado = await _unitOfWorkService.FixedRepo.FixedTermsOfUser(userId);
        await _unitOfWorkService.Complete();

        return resultado;
    }

}