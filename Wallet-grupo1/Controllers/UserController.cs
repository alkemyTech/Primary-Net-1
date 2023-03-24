using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;


        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok();
        }

       
    }
}
