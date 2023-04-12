using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers;

/// <summary>
/// Definición de controlador especializado para gestionar requests HTTP asociados a la entidad catálogos, ya sea
/// su inserción, su visualizacion total o parcial, su actualización o eliminación.
/// </summary>
[Route("api/catalogue")]
public class CatalogueController : Controller
{
    private readonly IUnitOfWork _unitOfWorkService;

    /// <summary>
    /// Constructor básico para el controlador el cual setea la unidad de trabajo a instanciar cada vez que un
    /// request HTTP es recibido por la API.
    /// </summary>
    public CatalogueController(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }

    /// <summary>
    /// Endpoint que provee la funcionalidad de insertar un catálogo a la base de datos.
    /// Requiere permisos de autenticacion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null) 
            return ResponseFactory.CreateErrorResponse(401,
                "No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return ResponseFactory.CreateErrorResponse(401,
                "No se proporcionó un token de seguridad válido.");
        
        // Carga todos los catálogos de la base de datos utilizando el repositorio de catálogos
        var catalogues = await _unitOfWorkService.CatalogueRepo.GetAll();
        
        // Paginar el resultado
        int pageToShow = 1;
        if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
        var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString(); 

        var paginatedCatalogues = PaginateHelper.Paginate(catalogues, pageToShow, url);    

        // Retorna un código 200 (OK) con la lista de catálogos paginada
        return ResponseFactory.CreateSuccessfullyResponse(200, paginatedCatalogues);
    }

    /// <summary>
    /// Endpoint que provee la funcionalidad de consultar una unidad de catálogo existente.
    /// Requiere permisos de autenticación y el codigo del catálogo a buscar.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        string? authorizationHeader = Request.Headers["Authorization"];

        if (authorizationHeader is null) 
            return ResponseFactory.CreateErrorResponse(401,
                "No se proporcionó un token de seguridad.");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            return ResponseFactory.CreateErrorResponse(401,
                "No se proporcionó un token de seguridad válido.");
        
        // Obtiene el catálogo con el ID especificado utilizando el repositorio de catálogos
        var catalogue = await _unitOfWorkService.CatalogueRepo.GetById(id);
        
        // Si no se encuentra un catálogo con el ID especificado, retorna un código 404 (Not Found)
        if (catalogue is null) 
            return ResponseFactory.CreateErrorResponse(404, "No se pudo localizar" +
                                                            $" al catalogo de ID: {id} en el sistema.");
        // Si se encuentra el catálogo, retorna un código 200 (OK) con el catálogo encontrado
        return ResponseFactory.CreateSuccessfullyResponse(200, catalogue);
    }

    /// <summary>
    /// Endpoint que provee la funcionalidad de insertar un catálogo a la base de datos.
    /// Requiere permisos de administrador y el codigo del catalogo a remover.
    /// </summary>
    /// <param name="catalogue"></param>
    /// <returns>Código de respuesta HTTP asociado al éxito o fracaso de la operación</returns>
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Catalogue catalogue)
    {
        if(catalogue.Points <= 0 || catalogue.Points == null)
            return ResponseFactory.CreateErrorResponse(500, "El catalogo ingresado no posee un precio en puntos valido.");
        
        if(catalogue.Image == null)
            return ResponseFactory.CreateErrorResponse(500, "El catalogo ingresado no posee una URL de imagen.");

        // Agrega el nuevo catálogo a la base de datos utilizando el repositorio de catálogos
        await _unitOfWorkService.CatalogueRepo.Insert(catalogue);
        await _unitOfWorkService.Complete();
        
        // Retorna un código 201 (Created) con el nuevo catálogo creado y su URL de ubicación
        return ResponseFactory.CreateSuccessfullyResponse(201, catalogue);
    }

    /// <summary>
    /// Endpoint que provee la funcionalidad de eliminar un catálogo del contexto de la API y la base de datos.
    /// Requiere permisos de administrador y el codigo del catalogo a remover.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Codigo de respuesta asociado al éxito o fracaso de la operación</returns>
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        //compruebo que el catalogo exista
        var catalogue = await _unitOfWorkService.CatalogueRepo.GetById(id);
        if (catalogue is null) 
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar el catálogo con id: {id}" +
                                                            $" porque no existe.");

        //intento remover el catalogo del contexto del repositorio en la api
        var result = await _unitOfWorkService.CatalogueRepo.Delete(catalogue);
        if (!result)
            return ResponseFactory.CreateErrorResponse(500, $"No se pudo eliminar el catálogo con id: {id}" +
                                                            $"porque no se pudo completar la transacción.");
        
        //aguardo el retorno del update en la DB y luego informo del exito
        await _unitOfWorkService.Complete();
        return ResponseFactory.CreateSuccessfullyResponse(200,
            $"Se eliminó el catálogo con ID: {catalogue.Id} satisfactoriamente del sistema.");
    }


    /// <summary>
    /// Endpoint que provee la funcionalidad de actualizar los datos de un catálogo de la base de datos.
    /// Requiere permisos de administrador y los datos actualizables pertinentes que se deseen ajustar:
    /// (puntos requeridos para canjear, la URL de la imagen referencial y un texto descriptivo del mismo)
    /// </summary>
    /// <param name="catalogue"></param>
    /// <returns>Codigo de respuesta HTTP asociado al éxito o fracaso del Request</returns>
    [Authorize(Policy = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Catalogue catalogue)
    {
        // Actualiza el catálogo especificado en la base de datos utilizando el repositorio de catálogos
        var result = await _unitOfWorkService.CatalogueRepo.Update(catalogue);
        if (!result)
            return ResponseFactory.CreateErrorResponse(404, $"No se pudo actualizar el catálogo con id: {id}" +
                                                            " porque no existe en el sistema.");
        // Guarda los cambios en la base de datos
        await _unitOfWorkService.Complete();
        // Retorna un código 200 (No Content) si la actualización fue exitosa
        return ResponseFactory.CreateSuccessfullyResponse(200,
            $"Se actualizo el catálogo con ID: {id} satisfactoriamente en el sistema.");
    }
}