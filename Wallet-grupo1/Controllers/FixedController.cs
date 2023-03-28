using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

public class FixedController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public FixedController(ApplicationDbContext context)
    {
        _context = context;
    }
    //Obtenemos todos los Fixed
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<FixedTermDeposit> Fixed;
        
        using (var uof = new UnitOfWork(_context))
        {
            Fixed = await uof.FixedRepo.GetAll();
        }

        return Ok(Fixed);
    }
    // Obtiene un Fixed mediante el ID
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        FixedTermDeposit? Fixed;

        using (var uof = new UnitOfWork(_context))
        {
            Fixed = await uof.FixedRepo.GetById(id);
        }

        if (Fixed is null) return NotFound();
        
        return Ok(Fixed);
    }
    
    [HttpPost]
    public IActionResult Insert([FromBody] FixedTermDeposit Fixed)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.FixedRepo.Insert(Fixed);
            uof.Complete();
        }  

        return CreatedAtAction(nameof(GetById), new { id = Fixed.Id}, Fixed);
    } 
     // Elimina un Fixed existente
    [HttpPost]
    public IActionResult Delete([FromBody] FixedTermDeposit Fixed)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.FixedRepo.Delete(Fixed);
            uof.Complete();
        }

        return Ok();
    }
    [HttpPut]

    ///Actualiza un Fixed existente
    public IActionResult Update([FromBody] FixedTermDeposit Fixed)
    {
        using (var uof = new UnitOfWork(_context))
        {
            uof.FixedRepo.Update(Fixed);
            uof.Complete();
        }

        return Ok();
    }

}