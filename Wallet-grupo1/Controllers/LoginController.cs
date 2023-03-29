using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("Login")]
public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private GestorTokenJwt _gestorToken;

    public LoginController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _gestorToken = new GestorTokenJwt(config);
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (!authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("No se proporcionó el header de autorización, por favor ingrese sus credenciales.");
        
        var token = authHeader.Substring("Basic ".Length).Trim();
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');

        User user;
        
        using (var uof = new UnitOfWork(_context))
        {
            user = await uof.UserRepo.AuthenticateCredentials(credentials[0], credentials[1]);
        }

        if (user is null) return Unauthorized("Las credenciales son incorrectas.");
        
        return Ok(_gestorToken.GenerateToken(user));
    }
}