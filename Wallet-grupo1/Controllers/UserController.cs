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

        /// <summary>
        /// Obtiene una lista paginada de todos los usuarios registrados.
        /// </summary>
        /// <returns>Una lista paginada de usuarios.</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _unitOfWorkService.UserRepo.GetAll();
            
            // Paginar el resultado
            int pageToShow = 1;
            if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
            var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString();

            var paginatedUsers = PaginateHelper.Paginate(users, pageToShow, url);  
            
            if(users == null)
            {
                return StatusCode(204, "No se encontraron usuarios");
            }

            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario en particular.
        /// </summary>
        /// <param name="id">El ID del usuario a obtener.</param>
        /// <returns>El usuario correspondiente al ID especificado.</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);
            if (user is null) return NotFound();

            return Ok(user);
        }


        /// <summary>
        /// Elimina un usuario existente.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        /// <returns>Un código de estado HTTP 200 (OK) si el usuario se eliminó correctamente.</returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);

            if (user is null) return NotFound($"No se encontró ningún usuario con el ID: {id}.");


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
                return StatusCode(500, $"No se pudo eliminar el usuario con ID: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

            await _unitOfWorkService.Complete();

            return Ok();
        }


        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="id">El ID del usuario a actualizar.</param>
        /// <param name="user">El objeto User con los datos actualizados.</param>
        /// <returns>Un código de estado HTTP 200 (OK) si el usuario se actualizó correctamente.</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UserDto user)
        {
            var result = await _unitOfWorkService.UserRepo.Update(new User(user,id));
            if (!result)
            
                return StatusCode(500, $"No se pudo actualizar el usuario con ID: {id}" +
                $" porque no existe o porque no se pudo completar la transacción.");
            
            await _unitOfWorkService.Complete();

            return Ok();
        }
        
        /// <summary>
        ///     Obtiene los datos del usuario logeado 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [Authorize]
        [HttpGet("authenticated")]
        public async Task<ActionResult<User>> AuthenticatedUserData([FromRoute] int id)
        {

            string? authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized("No se proporcionó un token de seguridad válido.");

            string jwtToken = authorizationHeader.Substring(7);

            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

            var user = await _unitOfWorkService.UserRepo.GetById(id);
            if (user is null) return NotFound($"No se encontró ningun usuario con el número: {id}.");

            if (user.Id != int.Parse(userIdToken))
                return Forbid("La cuenta no pertenece al usuario loggeado.");

            return Ok(user);
        }
    }
}
