//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

/// <summary>
/// Controlador para entidad roles del sistema.
/// </summary>
[Route("/api/role")]
public class RoleController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    /// <summary>
    /// Constructor base.
    /// </summary>
    public RoleController(IUnitOfWork unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    /// <summary>
    /// Obtener el listado de todos los roles del sistema. Solo usuarios administradores pueden acceder.
    /// </summary>
    /// <returns>Listado de todos los roles del sistema.</returns>
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rolesPresent = await _unitOfWorkService.RoleRepo.GetAll();

        return ResponseFactory.CreateSuccessfullyResponse(200, rolesPresent);
    }

    /// <summary>
    /// Obtener un rol a partir del ID especificado. Solo los administradores tienen acceso.
    /// </summary>
    /// <param name="id">ID del rol que se quiere recuperar.</param>
    /// <returns>El estado del rol con el ID especificado.</returns>
    [Authorize(Policy = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var roleToFind = await _unitOfWorkService.RoleRepo.GetById(id);

        if (roleToFind is null) 
            return ResponseFactory.CreateErrorResponse(404, $"No existe un rol en el sistema con ID: {id}");

        return ResponseFactory.CreateSuccessfullyResponse(200, roleToFind);
    }

    /// <summary>
    /// Insertar un rol en la base de datos con los datos pasados en el Body. Solo administradores tienen acceso.
    /// </summary>
    /// <param name="newRole">Estado en el que se quiere insertar el rol. El ID se autogenerará en la BD.</param>
    /// <returns>El resultado de la creación e inserción de la entidad y su estado.</returns>
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] RoleDto newRole)
    {
        if (!ModelState.IsValid)
            return ResponseFactory.CreateErrorResponse(500, "Hubo un problema insertando el nuevo rol en la DB");

        var result = await _unitOfWorkService.RoleRepo.Insert(new Role(newRole));

        if (!result) return ResponseFactory.CreateErrorResponse(500, "No se pudo insertar el nuevo Rol.");
        
        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(201, newRole);
    }

    /// <summary>
    /// Eliminar el rol de la base de datos cuyo ID corresponda con el especificado.
    /// Requiere permisos de admin.
    /// </summary>
    /// <param name="id">ID del rol que se desea eliminar.</param>
    /// <returns>Resultado de la transacción de eliminación</returns>
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var role = await _unitOfWorkService.RoleRepo.GetById(id);

        if (role is null || role.Id is 1 or 2) 
            return ResponseFactory.CreateErrorResponse(500, $"No es posible eliminar el rol con: {id} porque no existe" +
                "o es un rol fundamental del sistema");

        var result = await _unitOfWorkService.RoleRepo.Delete(role);
        if (!result)
            return StatusCode(500, $"No se pudo eliminar el rol con id: {id}" +
                                   "porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200, $"El rol con id: {id} fue eliminado del sistema");
    }

    /// <summary>
    /// Actualizar el estado de un rol con los datos pasados en el body.
    /// </summary>
    /// <param name="id">Información del rol a actualizar.</param>
    /// <param name="roleToUpdate">Información del rol a actualizar.</param>
    /// <returns>Resultado de la transacción de actualización.</returns>
    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Role roleToUpdate)
    {
        //solo poder cambiar la descripcion

        var result = await _unitOfWorkService.RoleRepo.Update(roleToUpdate);

        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar el rol con id: {id}" +
                                                                   $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return ResponseFactory.CreateSuccessfullyResponse(200, $"El rol con id: {id} fue actualizado");
    }
}