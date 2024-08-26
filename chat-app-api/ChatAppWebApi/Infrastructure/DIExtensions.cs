using Infrastructure.Database;
using Infrastructure.Database.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DIExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MessagesDbContext>((sc, options) => { options.UseNpgsql(configuration.GetConnectionString("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=InventoryManagement;Pooling=true;")); });

        services.AddIdentityCore<User>(config =>
        {
            config.User.RequireUniqueEmail = true;
            config.User.AllowedUserNameCharacters = null;
            config.Password.RequireDigit = true;
            config.Password.RequireLowercase = true;
            config.Password.RequireUppercase = true;
            config.Password.RequireNonAlphanumeric = true;
        })
            .AddRoles<Role>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<MessagesDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
