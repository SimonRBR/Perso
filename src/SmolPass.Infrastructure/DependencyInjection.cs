using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmolPass.Infrastructure.Persistence;

namespace SmolPass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String introuvable.");
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        
        }
    }
}
