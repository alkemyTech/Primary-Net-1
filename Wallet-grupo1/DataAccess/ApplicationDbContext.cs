using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<FixedTermDeposit> FixedTermDeposits => Set<FixedTermDeposit>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Catalogue> Catalogues => Set<Catalogue>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Defino que una cuenta solo puede estar asociada a un usuario, el que la crea
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            //Especifico las relaciones de navegacion para transaccion y mantengo un comportamiento de borrado en nulo
            //debido a que una fila de transaccion puede contener mas de una cuenta (la que mueve el dinero y la que lo recibe)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Defino que un plazo fijo solo puede estar asociado a una sola cuenta, la que lo genera
            modelBuilder.Entity<FixedTermDeposit>()
                .HasOne(f => f.Account)
                .WithMany(a => a.FixedTermDeposits)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.ClientCascade);
            
            //TODO Agregar aca validaciones por si el contexto de la DB no esta vacio para evitar que haga el seeding process
            
            var seeders = new List<IEntitySeeder>
            {
                new RoleSeeder(),
                new UserSeeder(),
                new AccountSeeder(),
                new CatalogueSeeder(),
                new FixedTermSeeder(),
                new TransactionSeeder()
            };

            foreach (var seeder in seeders)
            {
                seeder.SeedTheDatabase(modelBuilder);
            }
            
            //todas las demas relaciones son uno a uno asi que limitamos el uso de la fluent Api a estas relaciones
            base.OnModelCreating(modelBuilder); 
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(2);
            configurationBuilder.Properties<decimal>().HaveColumnType("decimal");
            configurationBuilder.Properties<string>().HaveMaxLength(20);
        }

       
        
    }
}
