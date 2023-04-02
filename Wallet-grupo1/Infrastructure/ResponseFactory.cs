using Microsoft.AspNetCore.Mvc;

namespace Wallet_grupo1.Infrastructure;

/// <summary>
/// Clase responseble de la creación de las respuestasa HTTP de la API con su respectivo formato.
/// </summary>
public static class ResponseFactory
{
    /// <summary>
    /// Crea una respuesta exitosa.
    /// </summary>
    /// <param name="statusCode">Codigo HTTP</param>
    /// <param name="data">Información de la respuesta</param>
    /// <returns>IActionResult con el codigo e información de respuesta</returns>
    public static IActionResult CreateSuccessfullyResponse(int statusCode, object? data)
    {
        var response = new ApiSuccessfullResponse()
        {
            Status = statusCode,
            Data = data
        };

        return new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Crea una respuesta de error.
    /// </summary>
    /// <param name="statusCode">Codigo de error HTTP</param>
    /// <param name="errors">Errores producidos a mostrar</param>
    /// <returns>IActionResult con codigo de error y infofmación de los errores que se produjeron</returns>
    public static IActionResult CreateErrorResponse(int statusCode, params string[] errors)
    {
        var response = new ApiErrorResponse()
        {
            Status = statusCode,
            Errors = new List<ApiErrorResponse.ResponseError>()
        };

        foreach (var error in errors)
        {
            response.Errors.Add(new ApiErrorResponse.ResponseError(){Error = error});
        }

        return new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }
}