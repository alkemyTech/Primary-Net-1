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

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);

            if (user is null) return NotFound($"No se encontro ningun user con el id: {id}.");

            // Busco el Id de la Account del User
            var account = await _unitOfWorkService.AccountRepo.FindByUserId(id);
            if (account is null) return NotFound($"No se encontro ningun account del user con el id: {id}.");

            // Elimino las Transaccion con Id la Account
            var deletedTransaccions = await _unitOfWorkService.TransactionRepo.RemoveReferencesToAccountId(account.Value.Id);
            if (!deletedTransaccions)
                return StatusCode(500, $"No se pudo eliminar la Transaccion del user con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

            // Elimino los FixedTermDeposit con la Id de la Account
            var deletedFixdTermDeposit = await _unitOfWorkService.FixedRepo.DeleteFixedTermsByAccount(account.Value.Id);
            if (!deletedFixdTermDeposit)
                return StatusCode(500, $"No se pudo eliminar FixedTerm del user con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

            // Elimino la Account con el Id del User
            var deletedAccount = await _unitOfWorkService.AccountRepo.Delete(account.Value);
            if (!deletedAccount)
                return StatusCode(500, $"No se pudo eliminar la account del user con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");


            // Elimino el User corrrespondiente
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
