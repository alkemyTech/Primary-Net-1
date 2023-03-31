using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.DatabaseSeeding;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

public class CatalogueSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Catalogue>().HasData(
            new Catalogue{ Id = 1, Name = "Producto1" , Image = "https://picsum.photos/id/812/200/300?grayscale&blur=2" , Points = 11},
            new Catalogue{ Id = 2, Name = "Producto2" , Image = "https://picsum.photos/id/84/200/300?grayscale&blur=2" , Points = 265},
            new Catalogue{ Id = 3, Name = "Producto3" , Image = "https://picsum.photos/id/12/200/300?grayscale&blur=2" , Points = 1251},
            new Catalogue{ Id = 4, Name = "Producto4" , Image = "https://picsum.photos/id/870/200/300?grayscale&blur=2" , Points = 5},
            new Catalogue{ Id = 5, Name = "Producto5" , Image = "https://picsum.photos/id/870/200/300?grayscale&blur=2" , Points = 11},
            new Catalogue{ Id = 6, Name = "Producto6" , Image = "https://picsum.photos/id/870/200/300?grayscale&blur=2" , Points = 11}
        );
    }
}