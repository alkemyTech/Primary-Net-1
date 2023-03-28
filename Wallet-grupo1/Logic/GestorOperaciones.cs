using Wallet_grupo1.Entidades;
using Wallet_grupo1.Services;

namespace Wallet_grupo1.Logic;

public class GestorOperaciones
{
    private readonly ApplicationDbContext _context;
    
    public GestorOperaciones(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Depositar(Account account, decimal aumentoSaldo, string concept)
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
            TransactionType = TransactionType.Deposit,
            Account = account,
            Date = DateTime.Now,
            Concept = concept
        };

        using (var uof = new UnitOfWork(_context))
        {
            await uof.TransactionRepo.Insert(transaction);
        }
    }

    public async Task Transferir(Account account, decimal aumentoSaldo, string concept)
    {
        throw new NotImplementedException();
    }
}