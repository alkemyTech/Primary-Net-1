using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet_grupo1.Services;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UserController(UnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser(User user)
        {
            if (!ModelState.IsValid) return new JsonResult("Something Went Wrong") { StatusCode = 500 };
            
            if(user.Password != null)
            {
                var newPass = PasswordEncrypt.EncryptPassword(user.Password).ToString();
                user.Password = newPass;
            }
            
            await _unitOfWork.UserRepo.Insert(user);
            await _unitOfWork.Complete();
            return CreatedAtAction("GetById", new { user.Id }, user);
        }
    
        [Authorize(Policy = "Admin")]
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

        [Authorize]
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
