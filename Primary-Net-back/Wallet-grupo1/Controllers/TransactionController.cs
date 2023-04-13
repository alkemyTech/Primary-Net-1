using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.AccessControl;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Infrastructure;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{
    /// <summary>
    /// Controlador para manejar requests asociadas a transacciones
    /// </summary>
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWorkService;

        /// <summary>
        /// Constructor base
        /// </summary>
        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWorkService = unitOfWork;
        }
        
        // /// <summary>
        // /// Se obtienen las transacciones del sistema. Requiere permisos de administrador.
        // /// </summary>
        // /// <returns></returns>
        // /// <exception cref="SecurityTokenException"></exception>
        // [Authorize(Policy = "Admin")]
        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     var transactions = await _unitOfWorkService.TransactionRepo.GetAll();
        //     
        //     if(transactions is null)
        //         return ResponseFactory.CreateErrorResponse(404, "No se pudieron localizar transacciones en el sistema.");
        //     
        //     // Paginar el resultado de Transaction
        //     int pageToShow = 1;
        //     if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
        //     var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString(); 
        //
        //     var paginatedTransactions = PaginateHelper.Paginate(transactions, pageToShow, url);
        //
        //     return ResponseFactory.CreateSuccessfullyResponse(200, paginatedTransactions);
        // }
        
        /// <summary>
        ///  Se obtiene una transaccion por ID de usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
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
            
            string jwtToken = authorizationHeader.Substring(7);

            // Extraigo el userid del token (es un claim)
            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");
            
            var userAccount = _unitOfWorkService.AccountRepo.FindByUserId(Int32.Parse(userIdToken));
            if (userAccount is null) 
                return ResponseFactory.CreateErrorResponse(403,$"No se encontró ninguna cuenta para el usuario de ID: {Int32.Parse(userIdToken)}.");
            
            var theTransaction = await _unitOfWorkService.TransactionRepo.GetById(id);
            if(theTransaction is null || theTransaction.AccountId != userAccount.Id)
                return ResponseFactory.CreateErrorResponse(500,$"No se encontró ninguna transaccion o bien la transaccion" +
                                                               $"especificada no fue efectuada por el usuario logueado");
            
            return ResponseFactory.CreateSuccessfullyResponse(200, theTransaction);
        }

        /// <summary>
        ///  Funcion para insertar una transaccion
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Transaction>> Insert(Transaction transaction)
        {
            //Realizo el Insert de la transaction
            await _unitOfWorkService.TransactionRepo.Insert(transaction);
            await _unitOfWorkService.Complete();
            
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id}, transaction);
        }
        
        /// <summary>
        /// Metodo para eliminar una transaccion. Requiere permisos de administrador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var transaction = await _unitOfWorkService.TransactionRepo.GetById(id);
            if (transaction is null)
                return ResponseFactory.CreateErrorResponse(404, $"No se encontró ninguna transaccion con el ID: {id}.");
            
            var result = await _unitOfWorkService.TransactionRepo.Delete(transaction);
            if (!result)
                return StatusCode(500, $"No se pudo eliminar la transacción con id: {id}" +
                                           " porque no existe o porque no se pudo completar la transacción.");
            
            await _unitOfWorkService.Complete();

            return ResponseFactory.CreateSuccessfullyResponse(200, $"Transaccion de ID: {id} eliminada satisfactoriamente del sistema.");
        }

        /// <summary>
        ///     Funcion para actualizar transaction, solo admins
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transactionToUpdate"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TransactionDto transactionToUpdate)
        {
            if(transactionToUpdate.Concept is null)
                return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar la transaccion con id: {id}" +
                                                                      " porque se necesita especificar el concepto que ha cambiado."); 
            
            var result = await _unitOfWorkService.TransactionRepo.Update(new Transaction(id, transactionToUpdate));
            
            if (!result)
                return ResponseFactory.CreateErrorResponse(500, $"No se pudo actualizar la transaccion con id: {id}" +
                                                                     $" porque no existe o porque no se pudo completar la transacción."); 
                                       
            await _unitOfWorkService.Complete();

            return ResponseFactory.CreateSuccessfullyResponse(200, $"Transaccion de ID: {id} actualizada satisfactoriamente.");
        }


        /// <summary>
        ///  Se obtienen todas las transacciones realizadas por un usuario
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TransactionsOfUser()
        {
            string? authorizationHeader = Request.Headers["Authorization"];
            if (authorizationHeader is null) 
                return ResponseFactory.CreateErrorResponse(401,
                    "No se proporcionó un token de seguridad.");

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return ResponseFactory.CreateErrorResponse(401,
                    "No se proporcionó un token de seguridad válido.");
            
            string jwtToken = authorizationHeader.Substring(7);

            // Extraigo el userid del token (es un claim)
            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");

            var userAccount = _unitOfWorkService.AccountRepo.FindByUserId(Int32.Parse(userIdToken)).Result;
            if (userAccount is null) 
                return ResponseFactory.CreateErrorResponse(403,$"No se encontró ninguna cuenta para el usuario de ID: {userIdToken}.");

            var allTransactionsOfUser = await _unitOfWorkService.TransactionRepo.TransactionsOfUser(userAccount.Value.Id);

            // Paginar el resultado de Transaction
            int pageToShow = 1;
            if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
            var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString(); 

            var paginatedTransactions = PaginateHelper.Paginate(allTransactionsOfUser, pageToShow, url);
            
            await _unitOfWorkService.Complete();

            return ResponseFactory.CreateSuccessfullyResponse(200, paginatedTransactions);
        }

    }
}
