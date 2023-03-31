using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

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
