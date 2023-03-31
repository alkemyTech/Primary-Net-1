using Wallet_grupo1.Entities;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Logic;

public class GestorOperaciones
{
    private readonly ApplicationDbContext _context;
    
    public GestorOperaciones(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Deposit(Account account, decimal aumentoSaldo, string concept)
    {
        // Actualizo saldo
        account.Money += aumentoSaldo;
        using (var uof = new UnitOfWork(_context))
        {
            await uof.AccountRepo.Update(account);
        }
        
        // Loggeamos la transaction
        var transaction = new Transaction()
        {
            Amount = aumentoSaldo,
            Type = TransactionType.Deposit,
            Account = account,
            Date = DateTime.Now,
            Concept = concept
        };

        using (var uof = new UnitOfWork(_context))
        {
            await uof.TransactionRepo.Insert(transaction);
        }
    }

    public async Task Transfer(Account account, Account toAccount, decimal montoTransferido, string concept)
    {
        // Actualizo saldo

        account.Money -= montoTransferido;
        toAccount.Money += montoTransferido;
    
        
        using (var uof = new UnitOfWork(_context))
        {
            await uof.AccountRepo.Update(account);
            await uof.AccountRepo.Update(toAccount);
        }

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

        using (var uof = new UnitOfWork(_context))
        {
            await uof.TransactionRepo.Insert(transaction);
        }
        
    }
    
    //
    //public bool ValidateUser(){
    //    
    //}
}