using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.AccessControl;
using Microsoft.IdentityModel.Tokens;
using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Helpers;
using Wallet_grupo1.Logic;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Controllers
{

    [Authorize] // solo usuarios autenticados pueden acceder a este controlador
    [Route("api/transaction")]

    public class TransactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWorkService;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWorkService = unitOfWork;
        }
        
        /// <summary>
        ///     Se obtienen transacciones
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll()
        {
            //Get token del header y validacion
            string? authorizationHeader = Request.Headers["Authorization"];
  
            if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized("No se proporcionó un token de seguridad válido.");
        
            string jwtToken = authorizationHeader.Substring(7);
        
            // Extraigo el userid del token (es un claim)
            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");
            
            var transactions = await _unitOfWorkService.TransactionRepo.TransactionsOfUser(int.Parse(userIdToken));
            
            // Paginar el resultado de Transaction
            int pageToShow = 1;
            if(Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);
            var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString(); 

            var paginatedTransaction = PaginateHelper.Paginate(transactions, pageToShow, url);


            return Ok(paginatedTransaction);
        }
        
        /// <summary>
        ///     Se obtiene transaccion por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById([FromRoute] int id)
        {
            //Get token del header y validacion
            string? authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader is null) return Unauthorized("No se proporcionó un token de seguridad.");

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized("No se proporcionó un token de seguridad válido.");
        
            string jwtToken = authorizationHeader.Substring(7);
        
            // Extraigo el userid del token (es un claim)
            var userIdToken = TokenJwtHelper.ObtenerUserIdDeToken(jwtToken);
            if (userIdToken is null) throw new SecurityTokenException("El token no tiene el claim del user id.");
            
            var transaction = await _unitOfWorkService.TransactionRepo.GetById(id);
            if (transaction is null) return NotFound();
            
            if(transaction.AccountId is null) return Forbid("El usuario loggeado no corresponde al del dueño de la cuenta.");
            var account = await _unitOfWorkService.AccountRepo.GetById(transaction.AccountId.Value);
            if (account is null) return NotFound();
            
            if (account.UserId != int.Parse(userIdToken))
                return Forbid("El usuario loggeado no corresponde al del dueño de la cuenta .");

            return Ok(transaction);
        }

        /// <summary>
        ///     Funcion para insertar transation
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
        ///     funcion para eliminar transaction. Solo pueden hacerlo los admins
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var transaction = await _unitOfWorkService.TransactionRepo.GetById(id);

            if (transaction is null) return NotFound($"No se encontro ninguna transacción con el id: {id}.");
                
            var result = await _unitOfWorkService.TransactionRepo.Delete(transaction);

            if (!result)
                return StatusCode(500, $"No se pudo eliminar la transacción con id: {id}" +
                                           $" porque no existe o porque no se pudo completar la transacción.");
            
            await _unitOfWorkService.Complete();

            return Ok();
        }

        /// <summary>
        ///     Funcion para actualizar transaction, solo admins
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {

            var result = await _unitOfWorkService.TransactionRepo.Update(transaction);
            
            if (!result)
                return StatusCode(500, $"No se pudo actualizar la transaccion con id: {transaction.Id}" +
                                           $" porque no existe o porque no se pudo completar la transacción."); 
                                       
            await _unitOfWorkService.Complete();

            return Ok();
        }


        /// <summary>
        ///     Se obtienen todas las transacciones realizadas por un usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<List<Transaction>> TransactionsOfUser([FromBody]int userId)
        {
            var resultado = await _unitOfWorkService.TransactionRepo.TransactionsOfUser(userId);

            await _unitOfWorkService.Complete();

            return resultado;
        }

    }
}
