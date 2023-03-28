//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("Role")]
public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public RoleController(ApplicationDbContext context)
    {
        _context = context;
    }
    
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
    
    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Role? roleToFind;

        using (var uof = new UnitOfWork(_context))
        {
            roleToFind = await uof.RoleRepo.GetById(id);
        }

        if (roleToFind is null) return NotFound();
        
        return Ok(roleToFind);
    }
    
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Role newRole)
    {
        using (var uof = new UnitOfWork(_context))
        {
            await uof.RoleRepo.Insert(newRole);
            await uof.Complete();
        }  

        return CreatedAtAction(nameof(GetById), new { id = newRole.Id}, newRole);
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