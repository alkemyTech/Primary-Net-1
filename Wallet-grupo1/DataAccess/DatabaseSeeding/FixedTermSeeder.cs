using Microsoft.EntityFrameworkCore;
using Primary_Wallet_pilot.DatabaseSeeding;
using Primary_Wallet_pilot.Entities;

namespace Primary_Wallet_pilot.DataAccess.DatabaseSeeding;

public class FixedTermSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FixedTermDeposit>().HasData(
            new FixedTermDeposit
            {
                Id = 1,
                Amount = 213, 
                CreationDate = DateTime.Today, 
                ClosingDate = new DateTime(2024,02,02),
                AccountId = 1
            },
            new FixedTermDeposit
            {
                Id = 2,
                Amount = 89921, 
                CreationDate = DateTime.Today, 
                ClosingDate = new DateTime(2024,05,15),
                AccountId = 4
            },
            new FixedTermDeposit
            {
                Id = 3,
                Amount = 22313, 
                CreationDate = DateTime.Today, 
                ClosingDate = new DateTime(2024,05,22),
                AccountId = 3
            }
        );
    }
}