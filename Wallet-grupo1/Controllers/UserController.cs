using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DataAccess.Repositories;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser(User user)
        {

            await _unitOfWork.UserRepo.Insert(user);
            await _unitOfWork.CompleteAsync();
              Unit.Dispose();

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _unitOfWork.UserRepo.GetAll();
            if (users != null)
            {
                return Ok(users);
            }
            else

            {

                return NotFound();
            }

            Unit.Dispose();
            return users;
            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _unitOfWork.UserRepo.GetById(id);

            if (user != null)
            {

                return Ok(user);
            }
            else

            {
                return NotFound();
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _unitOfWork.UserRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            await _unitOfWork.UserRepo.Delete(user);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _unitOfWork.UserRepo.GetById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Nombre = user.Nombre;
            existingUser.Apellido = user.Apellido;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.FechaNacimiento = user.FechaNacimiento;

            await _unitOfWork.CompleteAsync();


            return Ok();
        }
    }
}
