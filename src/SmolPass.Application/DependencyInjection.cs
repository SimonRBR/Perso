using Microsoft.Extensions.DependencyInjection;
using SmolPass.Application.UseCases.Auth;
using SmolPass.Application.UseCases.Vault;

namespace SmolPass.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterUserUseCase>();
            services.AddScoped<LoginInitUseCase>();
            services.AddScoped<LoginUserUseCase>();
            services.AddScoped<AddVaultItemUseCase>();
            services.AddScoped<GetVaultItemsUseCase>();
            services.AddScoped<GetVaultItemByIdUseCase>();
            services.AddScoped<UpdateVaultItemUseCase>();
            services.AddScoped<DeleteVaultItemUseCase>();
            return services;
        }
    }
}
