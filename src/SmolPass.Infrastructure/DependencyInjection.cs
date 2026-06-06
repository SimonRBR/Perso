using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmolPass.Application.Interfaces;
using SmolPass.Infrastructure.Persistence;
using SmolPass.Infrastructure.Repositories;

namespace SmolPass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String introuvable.");
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVaultItemRepository, VaultItemRepository>();


            return services;
        
        }
    }
}
