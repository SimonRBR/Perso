using Microsoft.EntityFrameworkCore;
using SmolPass.Domain.Entities;
using SmolPass.Infrastructure.Persistence.Converters;

namespace SmolPass.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
        { 
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<VaultItem> VaultItems => Set<VaultItem>();
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
