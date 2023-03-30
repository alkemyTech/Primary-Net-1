using Microsoft.EntityFrameworkCore;
using Primary_Wallet_pilot.DatabaseSeeding;
using Primary_Wallet_pilot.Entities;

namespace Primary_Wallet_pilot.DataAccess.DatabaseSeeding;

public class AccountSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account { Id = 1, CreationDate = new DateTime(2022,5,12), Money = 1034, IsBlocked = false, UserId = 1},
            new Account { Id = 2,  CreationDate = new DateTime(2011,2,22), Money = 1, IsBlocked = false, UserId = 2},
            new Account { Id = 3, CreationDate = DateTime.Today, Money = 104, IsBlocked = false, UserId = 3},
            new Account { Id = 4,CreationDate = new DateTime(2010,2,22), Money = 5551, IsBlocked = false, UserId = 4},
            new Account { Id = 5,CreationDate = DateTime.Today, Money = 19861, IsBlocked = false, UserId = 5},
            new Account { Id = 6,CreationDate = DateTime.Today, Money = 10134, IsBlocked = false, UserId = 6}
        );
    }
}