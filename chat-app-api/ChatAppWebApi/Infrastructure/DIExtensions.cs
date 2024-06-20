using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DIExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MessagesDbContext>((sc, options) => { options.UseNpgsql(configuration.GetConnectionString("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=InventoryManagement;Pooling=true;")); });

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
