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

        /// <summary>
        /// Obtiene una lista paginada de todos los usuarios registrados.
        /// </summary>
        /// <returns>Una lista paginada de usuarios.</returns>
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _unitOfWorkService.UserRepo.GetAll();
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
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var user = await _unitOfWorkService.UserRepo.GetById(id);

            if (user is null) return NotFound($"No se encontró ningún usuario con el ID: {id}.");

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
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User user)
        {
            var result = await _unitOfWorkService.UserRepo.Update(user);

            if (!result)
                return StatusCode(500, $"No se pudo actualizar el usuario con ID: {user.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

            await _unitOfWorkService.Complete();

            return Ok();
        }
    }
}
