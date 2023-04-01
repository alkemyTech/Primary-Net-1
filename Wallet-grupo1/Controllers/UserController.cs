using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Services;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;

namespace Wallet_grupo1.Controllers
{
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWorkService;

        public UserController(IUnitOfWork unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser(User user)
        {
            if (!ModelState.IsValid) return new JsonResult("Something Went Wrong") { StatusCode = 500 };
            
            var newPass = PasswordEncryptHelper.EncryptPassword(user.Password);
            user.Password = newPass;

            await _unitOfWorkService.UserRepo.Insert(user);
            await _unitOfWorkService.Complete();
            return CreatedAtAction("GetById", new { user.Id }, user);
        }
    
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _unitOfWorkService.UserRepo.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);
            if (user is null) return NotFound();
        
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);

            if (user is null) return NotFound($"No se encontro ningun user con el id: {id}.");
                
            var result = await _unitOfWorkService.UserRepo.Delete(user);

            if (!result)
                return StatusCode(500, $"No se pudo eliminar el user con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");
                                       
            await _unitOfWorkService.Complete();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User user)
        {
            var result = await _unitOfWorkService.UserRepo.Update(user);
            
            if (!result)
                return StatusCode(500, $"No se pudo actualizar el user con id: {user.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción."); 
                                       
            await _unitOfWorkService.Complete();
            
            return Ok();
        }
    }
}
