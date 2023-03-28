using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DataAccess.Repositories;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser(User user)
        {
            using (var uof = new UnitOfWork(_context))
            {
                await uof.UserRepo.Insert(user);
                await uof.Complete();
            }  

            return CreatedAtAction(nameof(GetById), new { id = user.Id}, user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            List<User> users;
        
            using (var uof = new UnitOfWork(_context))
            {
                users = await uof.UserRepo.GetAll();
            }

            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById([FromRoute] int id)
        {
            User? user;

            using (var uof = new UnitOfWork(_context))
            {
                user = await uof.UserRepo.GetById(id);
            }

            if (user is null) return NotFound();
        
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            
            using (var uof = new UnitOfWork(_context))
            {
                var user = await uof.UserRepo.GetById(id);

                if (user is null) return NotFound($"No se encontro ningun user con el id: {id}.");
                
                var result = await uof.UserRepo.Delete(user);

                if (!result)
                    return StatusCode(500, $"No se pudo eliminar el user con id: {id}" +
                                           $" porque no existe o porque no se pudo completar la transacción.");
                                       
                await uof.Complete();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User user)
        {
            using (var uof = new UnitOfWork(_context))
            {
                var result = await uof.UserRepo.Update(user);
            
                if (!result)
                    return StatusCode(500, $"No se pudo actualizar el user con id: {user.Id}" +
                                           $" porque no existe o porque no se pudo completar la transacción."); 
                                       
                await uof.Complete();
            }
            
            return Ok();
        }
    }
}
