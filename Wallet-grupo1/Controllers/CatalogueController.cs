using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

[Route("Catalogue")]
public class CatalogueController : Controller
{
    private readonly ApplicationDbContext _context;

    public CatalogueController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Obtiene todos los catálogos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Catalogue> catalogues;
        using (var uof = new UnitOfWork(_context))
        {
            // Carga todos los catálogos de la base de datos utilizando el repositorio de catálogos
            catalogues = await uof.CatalogueRepo.GetAll();
        }
        // Retorna un código 200 (OK) con la lista de catálogos
        return Ok(catalogues);
    }

    // Obtiene un catálogo por su ID
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        Catalogue? catalogue;
        using (var uof = new UnitOfWork(_context))
        {
            // Obtiene el catálogo con el ID especificado utilizando el repositorio de catálogos
            catalogue = await uof.CatalogueRepo.GetById(id);
        }
        // Si no se encuentra un catálogo con el ID especificado, retorna un código 404 (Not Found)
        if (catalogue is null) return NotFound();
        // Si se encuentra el catálogo, retorna un código 200 (OK) con el catálogo encontrado
        return Ok(catalogue);
    }

    // Crea un nuevo catálogo
    [HttpPost]
    public IActionResult Create([FromBody] Catalogue catalogue)
    {
        using (var uof = new UnitOfWork(_context))
        {
            // Agrega el nuevo catálogo a la base de datos utilizando el repositorio de catálogos
            uof.CatalogueRepo.Insert(catalogue);
            // Guarda los cambios en la base de datos
            uof.Complete();
        }
        // Retorna un código 201 (Created) con el nuevo catálogo creado y su URL de ubicación
        return CreatedAtAction(nameof(GetById), new { id = catalogue.Id }, catalogue);
    }

    // Elimina un catálogo existente
    [HttpPost]
    public IActionResult Delete([FromBody] Catalogue catalogue)
    {
        using (var uof = new UnitOfWork(_context))
        {
            // Elimina el catálogo especificado de la base de datos utilizando el repositorio de catálogos
            uof.CatalogueRepo.Delete(catalogue);
            // Guarda los cambios en la base de datos
            uof.Complete();
        }
        // Retorna un código 204 (No Content) si la eliminación fue exitosa
        return NoContent();
    }

    // Actualiza un catálogo existente
    [HttpPut]
    public IActionResult Update([FromBody] Catalogue catalogue)
    {
        using (var uof = new UnitOfWork(_context))
        {
            // Actualiza el catálogo especificado en la base de datos utilizando el repositorio de catálogos
            uof.CatalogueRepo.Update(catalogue);
            // Guarda los cambios en la base de datos
            uof.Complete();
        }
        // Retorna un código 204 (No Content) si la actualización fue exitosa
        return NoContent();
    }
}