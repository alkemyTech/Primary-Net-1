using System.Collections.Immutable;
using Azure.Core;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Helpers;

public static class PaginateHelper
{
    /// <summary>
    /// Se encarga de paginar una lista de elementos T (generico). Pagina de a 10 elementos por pagina.
    /// </summary>
    /// <param name="itemsToPaginate">Lista a paginar</param>
    /// <param name="currentPage">Pagina que se desea mostrar</param>
    /// <param name="url">URL de la API para mostrar pagina anterior y siguiente</param>
    /// <typeparam name="T">El tipo de los elementos que se van a paginar</typeparam>
    /// <returns>Lista paginada de a 10 elementos por pagina y su respectiva información
    /// (pagina siguiente y anterior, pagina actual, elementos en la pagina, total de paginas)</returns>
    public static PaginateDataDto<T> Paginate<T>(List<T> itemsToPaginate, int currentPage, string url)
    {
        int pageSize = 10;

        var totalItems = itemsToPaginate.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        
        var paginatedItems = itemsToPaginate.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        var prevUrl = currentPage > 1 ? $"{url}?page={currentPage - 1}" : null;
        var nextUrl = currentPage < totalPages ? $"{url}?page={currentPage + 1}" : null;

        return new PaginateDataDto<T>()
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalItems = totalItems,
            PrevUrl = prevUrl,
            NextUrl = nextUrl,
            Items = paginatedItems
        };
    }
}