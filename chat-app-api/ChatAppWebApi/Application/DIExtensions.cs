using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SHG.Application;
using System.Reflection;

namespace Application;

public static class DIExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddScoped<TokenService>();

        return services;
    }
}
