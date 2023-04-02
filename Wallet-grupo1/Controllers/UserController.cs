using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Services;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.DTOs;
using Microsoft.IdentityModel.Tokens;

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
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UserDto user)
        {
            string? authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized("No se proporcionó un token de seguridad válido.");

            string jwtToken = authorizationHeader.Substring(7);

            // Extraigo el userid del token (es un claim)
            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");



            var userToken = _unitOfWorkService.AccountRepo.GetById(id).Result;

            if (userToken is null) return NotFound($"No se encontró ningun user con el id: {id}.");

            // Valido que sea el mismo user el loggeado y el dueño de la cuenta.
            if (userToken.UserId != id)
                return Forbid("La cuenta no pertenece al usuario loggeado.");





            var result = await _unitOfWorkService.UserRepo.Update(new Entities.User(user, id));


            if (!result)
                return StatusCode(500, $"No se pudo actualizar el user con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

            await _unitOfWorkService.Complete();

            return Ok();
        }
    }
}
