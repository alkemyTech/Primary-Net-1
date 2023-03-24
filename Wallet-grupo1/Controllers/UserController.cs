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
        private readonly ApplicationDbContext _context;


        public UserController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            var unit = new UnitOfWork(_context);
            try
            {
                unit.UserRepo.Insert(user);
                unit.complete();

            }catch(Exception ex)
            {

            }
            finally
            {
                unit.Dispose();
            }

            return Ok();
        }

       
    }
}
