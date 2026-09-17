using Microsoft.Extensions.DependencyInjection;
using Trusted.Application.Services;
using Trusted.Core.IServices;

namespace Trusted.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
