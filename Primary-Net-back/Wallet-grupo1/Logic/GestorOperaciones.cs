using Wallet_grupo1.DataAccess;
using Wallet_grupo1.Entities;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Logic;

/// <summary>
/// Clase dedicada a logica de negocio
/// </summary>
public class GestorOperaciones
{
    private readonly IUnitOfWork _unitOfWorkService;
    
    /// <summary>
    /// Constructor base del gestor
    /// </summary>
    public GestorOperaciones(IUnitOfWork unitOfWork)
    {
        _unitOfWorkService = unitOfWork;
    }

    /// <summary>
    /// Logica para realizar depositos de parte de un usuario a su cuenta
    /// </summary>
    public async Task Deposit(Account account, decimal aumentoSaldo, string concept)
    {
        // Actualizo saldo
        account.Money += aumentoSaldo;
        await _unitOfWorkService.AccountRepo.Update(account);
        
        // Loggeamos la transaction
        var transaction = new Transaction()
        {
            Amount = aumentoSaldo,
            Type = TransactionType.Deposit,
            Account = account,
            Date = DateTime.Now,
            Concept = concept
        };
        
        await _unitOfWorkService.TransactionRepo.Insert(transaction);
    }

    /// <summary>
    /// Logica para realizar transferencias de parte de un usuario a la cuenta de otro
    /// </summary>
    public async Task Transfer(Account account, Account toAccount, decimal montoTransferido, string concept)
    {
        // Actualizo saldo
        account.Money -= montoTransferido;
        toAccount.Money += montoTransferido;
        
        await _unitOfWorkService.AccountRepo.Update(account);
        await _unitOfWorkService.AccountRepo.Update(toAccount);
        
        // Loggeamos la transaction
        var transaction = new Transaction()
        {
            Amount = montoTransferido,
            Type = TransactionType.Payment,
            Account = account,
            //toAccount = toAccount,
            Date = DateTime.Now,
            Concept = concept
        };
        
        await _unitOfWorkService.TransactionRepo.Insert(transaction);
        await _unitOfWorkService.Complete();
    }

    /// <summary>
    /// Logica para crear un plazo fijo de parte de un usuario con dinero de su cuenta
    /// </summary>
    public async Task CreateFixedTerm(Account userAccount, FixedTermDeposit theNewFixedTerm, int interestWage)
    {
        userAccount.Money -= theNewFixedTerm.Amount; 
        theNewFixedTerm.Amount += ((theNewFixedTerm.Amount) * interestWage) / 100;
        
        await _unitOfWorkService.AccountRepo.Update(userAccount);
        await _unitOfWorkService.FixedRepo.Insert(theNewFixedTerm);
        await _unitOfWorkService.Complete();
    }
}