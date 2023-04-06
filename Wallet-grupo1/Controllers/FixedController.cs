using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

/// <summary>
/// Controlador que provee la funcionalidades asociadas a los plazos fijos/inversiones con la wallet.
/// </summary>
[Route("api/deposit")]
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
            return StatusCode(204, "No se encontraron FixedTermDeposit");
        }
        // Retorna un código 200 (OK)
        return Ok(paginatedFixed);
    }

    /// <summary>
    /// Endpoint que lista los detalles de un plazo fijo segun su código identificador.
    /// <param name="id">ID del plazo fijo que se desea consultar.</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    /// </summary>
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
    /// <param name="Fixed">El constructor del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    /// </summary>
    [Authorize]
    [HttpPost]
    public IActionResult Insert([FromBody] FixedTermDeposit Fixed)
    {
        _unitOfWorkService.FixedRepo.Insert(Fixed);
        _unitOfWorkService.Complete();

        return CreatedAtAction(nameof(GetById), new { id = Fixed.Id }, Fixed);
    }
    
    /// <summary>
    /// Endpoint que elimina la fila de un plazo fijo a partir del código identificador.
    /// <param name="id">El identificador del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    /// </summary>
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
    /// <param name="Fixed">El constructor del plazo fijo</param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    /// </summary>
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
    /// <param name="userId"></param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<List<FixedTermDeposit>> FixedTermsOfUser([FromBody] int userId)
    {
        var resultado = await _unitOfWorkService.FixedRepo.FixedTermsOfUser(userId);
        await _unitOfWorkService.Complete();

        return resultado;
    }

}