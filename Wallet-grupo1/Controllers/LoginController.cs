using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("api")]
public class LoginController : Controller
{
    private TokenJwtHelper _token;
    private readonly IUnitOfWork _unitOfWork;

    public LoginController(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _token = new TokenJwtHelper(config);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (!authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("No se proporcionó el header de autorización, por favor ingrese sus credenciales.");
        
        var token = authHeader.Substring("Basic ".Length).Trim();
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');

        
        var userCredentials = await _unitOfWork.UserRepo.AuthenticateCredentials(credentials[0], credentials[1]);

        if (userCredentials is null) return Unauthorized("Las credenciales son incorrectas.");
        
        return Ok(_token.GenerateToken(userCredentials));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        await _unitOfWork.UserRepo.Insert(user);

        var token = _token.GenerateToken(user);

        return Ok(token);
    }
}