using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Controllers
{
    [ApiController]
    [Route("/api/catalogo")]
    public class CatalogoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CatalogoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Catalogue>>> GetAll()
        {
            return await context.Catalogues.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Catalogue>> GetById(int id)
        {
            var catalogo = await context.Catalogues.FindAsync(id);

            if (catalogo is null)
            {
                return NotFound();
            }

            return catalogo;
        }

        [HttpPost]
        public async Task<ActionResult<Catalogue>> Insert(Catalogue catalogo)
        {
            context.Catalogues.Add(catalogo);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = catalogo.Id }, catalogo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Catalogue catalogo)
        {
            if (id != catalogo.Id)
            {
                return BadRequest();
            }

            context.Entry(catalogo).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var catalogo = await context.Catalogues.FindAsync(id);

            if (catalogo is null)
            {
                return NotFound();
            }

            context.Catalogues.Remove(catalogo);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatalogoExists(int id)
        {
            return context.Catalogues.Any(c => c.Id == id);
        }
    }
}