using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
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
        return Ok(Fixed);
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

        return CreatedAtAction(nameof(GetById), new { id = Fixed.Id}, Fixed);
    } 
     // Elimina un Fixed existente
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var fixedTermDeposit = await _unitOfWorkService.FixedRepo.GetById(id);

        if (fixedTermDeposit is null) return NotFound($"No se encontro ningun plazo fijo con el id: {id}.");
        
        var result = await _unitOfWorkService.FixedRepo.Delete(fixedTermDeposit);

        if (!result)
            return StatusCode(500, $"No se pudo eliminar el plazo fijo con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");
                                       
        await _unitOfWorkService.Complete();
        
        return Ok();
    }
    [HttpPut]

    ///Actualiza un Fixed existente
    public IActionResult Update([FromBody] FixedTermDeposit Fixed)
    {
        _unitOfWorkService.FixedRepo.Update(Fixed);
        _unitOfWorkService.Complete();
        
        return Ok();
    }
    
    [HttpGet("{userId}")]
    public async Task<List<FixedTermDeposit>> FixedTermsOfUser([FromBody]int userId)
    {
        var resultado = await _unitOfWorkService.FixedRepo.FixedTermsOfUser(userId);
        await _unitOfWorkService.Complete();

        return resultado;

    }

}