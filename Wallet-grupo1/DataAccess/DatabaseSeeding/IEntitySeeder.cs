using Microsoft.EntityFrameworkCore;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

public interface IEntitySeeder
{
    //La interfaz de Seeder generica que facilita escalabilidad, TODO clase abstracta potencial refactor
    void SeedTheDatabase(ModelBuilder modelBuilder);
}