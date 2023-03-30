using Microsoft.EntityFrameworkCore;

namespace Primary_Wallet_pilot.DatabaseSeeding;

public interface IEntitySeeder
{
    //La interfaz de Seeder generica que facilita escalabilidad, TODO clase abstracta potencial refactor
    void SeedTheDatabase(ModelBuilder modelBuilder);
}