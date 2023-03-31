//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("/api/role")]
public class RoleController : Controller
{
    private readonly UnitOfWorkService _unitOfWorkService;
    
    public RoleController(UnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rolesPresent = await _unitOfWorkService.RoleRepo.GetAll();

        return Ok(rolesPresent);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var roleToFind = await _unitOfWorkService.RoleRepo.GetById(id);
        
        if (roleToFind is null) return NotFound();
        
        return Ok(roleToFind);
    }
    
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Role newRole)
    {
        if (!ModelState.IsValid)
            return new JsonResult("Hubo un problema insertando el nuevo rol en la DB") { StatusCode = 500 };
        
        await _unitOfWorkService.RoleRepo.Insert(newRole);
        await _unitOfWorkService.Complete();

        return CreatedAtAction("GetById", new { newRole.Id }, newRole);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var role = await _unitOfWorkService.RoleRepo.GetById(id);

        if (role is null) return NotFound($"No se encontro ninguna transacción con el id: {id}.");
        
        var result = await _unitOfWorkService.RoleRepo.Delete(role);

        if (!result)
            return StatusCode(500, $"No se pudo eliminar el rol con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");
                                       
        await _unitOfWorkService.Complete();
        
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Role roleToUpdate)
    {
        var result = await _unitOfWorkService.RoleRepo.Update(roleToUpdate);
            
        if (!result)
            return StatusCode(500, $"No se pudo actualizar el rol con id: {roleToUpdate.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción."); 
                                       
        await _unitOfWorkService.Complete();

        return Ok();
    }
}