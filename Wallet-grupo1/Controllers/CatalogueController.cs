using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("api/catalogue")]
public class CatalogueController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    public CatalogueController(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }

    // Obtiene todos los catálogos
    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Carga todos los catálogos de la base de datos utilizando el repositorio de catálogos
        var catalogues = await _unitOfWorkService.CatalogueRepo.GetAll();
        
        // Paginar el resultado
        int pageToShow = 1;
        if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
        var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString(); 

        var paginatedCatalogues = PaginateHelper.Paginate(catalogues, pageToShow, url);    

        // Retorna un código 200 (OK) con la lista de catálogos paginado
        return Ok(paginatedCatalogues);
    }

    // Obtiene un catálogo por su ID
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // Obtiene el catálogo con el ID especificado utilizando el repositorio de catálogos
        var catalogue = await _unitOfWorkService.CatalogueRepo.GetById(id);
        
        // Si no se encuentra un catálogo con el ID especificado, retorna un código 404 (Not Found)
        if (catalogue is null) return NotFound();
        // Si se encuentra el catálogo, retorna un código 200 (OK) con el catálogo encontrado
        return Ok(catalogue);
    }

    // Crea un nuevo catálogo
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Catalogue catalogue)
    {
        // Agrega el nuevo catálogo a la base de datos utilizando el repositorio de catálogos
        await _unitOfWorkService.CatalogueRepo.Insert(catalogue);
        await _unitOfWorkService.Complete();
        
        // Retorna un código 201 (Created) con el nuevo catálogo creado y su URL de ubicación
        return CreatedAtAction(nameof(GetById), new { id = catalogue.Id }, catalogue);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var catalogue = await _unitOfWorkService.CatalogueRepo.GetById(id);

        if (catalogue is null) return NotFound($"No se encontro ningun catálogo con el id: {id}.");

        var result = await _unitOfWorkService.CatalogueRepo.Delete(catalogue);

        if (!result)
            return StatusCode(500, $"No se pudo eliminar el catálogo con id: {id}" +
                                   $" porque no existe o porque no se pudo completar la transacción.");

        await _unitOfWorkService.Complete();

        return Ok();
    }

    // Actualiza un catálogo existente
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Catalogue catalogue)
    {
        // Actualiza el catálogo especificado en la base de datos utilizando el repositorio de catálogos
        await _unitOfWorkService.CatalogueRepo.Update(catalogue);
        // Guarda los cambios en la base de datos
        await _unitOfWorkService.Complete();
        // Retorna un código 204 (No Content) si la actualización fue exitosa
        return NoContent();
    }
}