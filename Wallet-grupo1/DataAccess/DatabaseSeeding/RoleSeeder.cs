using Microsoft.EntityFrameworkCore;
using Primary_Wallet_pilot.DatabaseSeeding;
using Primary_Wallet_pilot.Entities;

namespace Primary_Wallet_pilot.DataAccess.DatabaseSeeding;

public class RoleSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role{ Id = 1, Name = "Admin" , Description = "This role has total functional control over the system"},
            new Role{ Id = 2, Name = "Regular" , Description = "This role has all nominal business functionalities"}
        );
    }
}
