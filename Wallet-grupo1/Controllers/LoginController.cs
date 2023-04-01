using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
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
    
    /// <summary>
    /// Endpoint encargado del logueo de un usuario en el sistema. Cualquier usuario del sistema puede acceder a el.
    /// </summary>
    /// <returns>Un token de autorización necesario para el uso del resto del sistema.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        if (!authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("No se proporcionó el header de autorización, por favor ingrese sus credenciales.");
        
        var token = authHeader.Substring("Basic ".Length).Trim();
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');
        credentials[1] = PasswordEncryptHelper.EncryptPassword(credentials[1]);
        
        var userCredentials = await _unitOfWork.UserRepo.AuthenticateCredentials(credentials[0], credentials[1]);

        if (userCredentials is null) return Unauthorized("Las credenciales son incorrectas.");
        
        return Ok(_token.GenerateToken(userCredentials));
    }
    
    /// <summary>
    /// Endpoint encargado del registro de nuevos usuarios. Cualquier usuario del sistema puede accederlo.
    /// </summary>
    /// <param name="dto">Datos necesarios: FirstName, LastName, Email y Password</param>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new User(dto);

        if (await _unitOfWork.UserRepo.ExisteUsuario(user)) return ResponseFactory.CreateErrorResponse(409,
            "Ya existe un usuario registrado con ese mail o nombre y apellido.");
        
        await _unitOfWork.UserRepo.Insert(user);
        await _unitOfWork.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(201, "Usuario registrado con éxito!");
    }
}