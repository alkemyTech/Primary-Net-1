using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

public class TransactionSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasData(
            new Transaction { Id = 1, Type = TransactionType.Deposit, Amount = 1314, Concept = "Deposito1", Date = DateTime.Today, AccountId = 1},
            new Transaction { Id = 2, Type = TransactionType.Deposit, Amount = 513514, Concept = "Deposito2", Date = DateTime.Today, AccountId = 1},
            new Transaction { Id = 3, Type = TransactionType.Deposit, Amount = 67113, Concept = "Deposito3", Date = DateTime.Today, AccountId = 1},
            new Transaction { Id = 4, Type = TransactionType.Payment, Amount = 78, Concept = "Pago1", Date = DateTime.Today, AccountId = 1, ToAccountId = 2},
            new Transaction { Id = 5, Type = TransactionType.Payment, Amount = 125, Concept = "Pago2", Date = DateTime.Today, AccountId = 3, ToAccountId = 1},
            new Transaction { Id = 6, Type = TransactionType.Payment, Amount = 6213, Concept = "Pago3", Date = DateTime.Today, AccountId = 6, ToAccountId = 5}
            );
    }
}