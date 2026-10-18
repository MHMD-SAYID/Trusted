using Microsoft.Extensions.DependencyInjection;
using Trusted.Application.Services;
using Trusted.Core.IRepositories;
using Trusted.Core.IServices;
using Trusted.Infrastructure.Repositories;

namespace Trusted.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();


        return services;
    }
}
