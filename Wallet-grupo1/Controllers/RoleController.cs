//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[ApiController]
[Route("/api/role")]
public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    
    public RoleController(UnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Role> rolesPresent;
        
        using (var uof = new UnitOfWork(_context))
        {
            rolesPresent = await uof.RoleRepo.GetAll();
        }

        return Ok(rolesPresent);
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var roleToFind = await _unitOfWork.RoleRepo.GetById(id);
        
        if (roleToFind is null) return NotFound();
        
        return Ok(roleToFind);
    }
    
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Role newRole)
    {
        if (!ModelState.IsValid)
            return new JsonResult("Hubo un problema insertando el nuevo rol en la DB") { StatusCode = 500 };
        
        await _unitOfWork.RoleRepo.Insert(newRole);
        await _unitOfWork.Complete();

        return CreatedAtAction("GetById", new { newRole.Id }, newRole);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] Role roleToDelete)
    {
        using (var uof = new UnitOfWork(_context))
        {
            var result = await uof.RoleRepo.Delete(roleToDelete);

            if (!result)
                return StatusCode(500, $"No se pudo eliminar el rol con id: {roleToDelete.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");
                                       
            await uof.Complete();
        }
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Role roleToUpdate)
    {
        using (var uof = new UnitOfWork(_context))
        {
            var result = await uof.RoleRepo.Update(roleToUpdate);
            
            if (!result)
                return StatusCode(500, $"No se pudo actualizar el rol con id: {roleToUpdate.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción."); 
                                       
            await uof.Complete();
        }

        return Ok();
    }
}