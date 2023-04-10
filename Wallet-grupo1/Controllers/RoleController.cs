//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("/api/role")]
public class RoleController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

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

        return Ok(rolesPresent);
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

        if (roleToFind is null) return NotFound();

        return Ok(roleToFind);
    }

    /// <summary>
    /// Insertar un rol en la base de datos con los datos pasados en el Body.
    /// </summary>
    /// <param name="newRole">Estado en el que se quiere insertar el rol. El ID se autogenerará en la BD.</param>
    /// <returns>El resultado de la creación e inserción de la entidad y su estado.</returns>
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Role newRole)
    {
        if (!ModelState.IsValid)
            return new JsonResult("Hubo un problema insertando el nuevo rol en la DB") { StatusCode = 500 };

        await _unitOfWorkService.RoleRepo.Insert(newRole);
        await _unitOfWorkService.Complete();

        return CreatedAtAction("GetById", new { newRole.Id }, newRole);
    }

    /// <summary>
    /// Eliminar el rol de la base de datos cuyo ID corresponda con el especificado.
    /// </summary>
    /// <param name="id">ID del rol que se desea eliminar.</param>
    /// <returns>Resultado de la transacción de eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var role = await _unitOfWorkService.RoleRepo.GetById(id);

        if (role is null) return NotFound($"No se encontro ninguna transacción con el id: {id}.");

        var result = await _unitOfWorkService.RoleRepo.Delete(role);

        if (!result)
            return StatusCode(500, $"No se pudo eliminar el rol con id: {id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return Ok();
    }

    /// <summary>
    /// Actualizar el estado de un rol con los datos pasados en el body.
    /// </summary>
    /// <param name="roleToUpdate">Información del rol a actualizar.</param>
    /// <returns>Resultado de la transacción de actualización.</returns>
    [Authorize(Policy = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Role roleToUpdate)
    {
        //solo poder cambiar la descripcion

        var result = await _unitOfWorkService.RoleRepo.Update(roleToUpdate);

        if (!result)
            return StatusCode(500, $"No se pudo actualizar el rol con id: {roleToUpdate.Id}" +
                                       $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return Ok();
    }
}