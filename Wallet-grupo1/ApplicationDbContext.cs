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
            
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(2);
            configurationBuilder.Properties<decimal>().HaveColumnType("decimal");
            configurationBuilder.Properties<string>().HaveMaxLength(20);
        }

       
        
    }
}
