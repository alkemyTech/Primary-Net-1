using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DataAccess.Repositories;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : Controller
    {

        IUnitOfWork Unit;

        public UserController(IUnitOfWork unit)
        {
            this.Unit = unit;
        }

        [HttpPost]
        public async Task<ActionResult> InsertUser(User user)
        {
            await Unit.UserRepo.Insert(user);
            await Unit.Complete();
            return Ok(user);
        }

        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await Unit.UserRepo.GetAll();   
            if(users != null)
            {
                return users;
            }else
            {
                return NotFound();
            }
        }

        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await Unit.UserRepo.GetById(id);

            if(user != null)
            {
                return user;
            }else
            {
                return NotFound();
            }    
        }

        public async Task<ActionResult> Delete(User user)
        {
            await Unit.UserRepo.Delete(user);
            await Unit.Complete();
            return Ok();
        }

        public async Task<ActionResult> Update(User user)
        {
            await Unit.UserRepo.Update(user);
            await Unit.Complete();
            return Ok();
        }
       
    }
}
