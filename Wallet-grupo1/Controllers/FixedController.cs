using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("api/deposit")]
public class FixedController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    public FixedController(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }
    //Obtenemos todos los Fixed
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
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
     
        return Ok(paginatedFixed);
    }

    // Obtiene un Fixed mediante el ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var Fixed = await _unitOfWorkService.FixedRepo.GetById(id);

        if (Fixed is null) return NotFound();

        return Ok(Fixed);
    }

    [HttpPost]
    public IActionResult Insert([FromBody] FixedTermDeposit Fixed)
    {
        _unitOfWorkService.FixedRepo.Insert(Fixed);
        _unitOfWorkService.Complete();

        return CreatedAtAction(nameof(GetById), new { id = Fixed.Id }, Fixed);
    }
    // Elimina un Fixed existente
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFixedTermDeposit([FromRoute] int id)
    {
        var fixedTermDeposit = await _unitOfWorkService.FixedRepo.GetById(id);

        if (fixedTermDeposit is null) return ResponseFactory.CreateErrorResponse(404, $"No se encontró ningún plazo fijo con el id: {id}.");

        var result = await _unitOfWorkService.FixedRepo.Delete(fixedTermDeposit);

        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar el plazo fijo con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200, "El plazo fijo se eliminó con éxito.");
    }


    ///Actualiza un Fixed existente
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

    [HttpGet("{userId}")]
    public async Task<List<FixedTermDeposit>> FixedTermsOfUser([FromBody] int userId)
    {
        var resultado = await _unitOfWorkService.FixedRepo.FixedTermsOfUser(userId);
        await _unitOfWorkService.Complete();

        return resultado;

    }

}