using Microsoft.EntityFrameworkCore;
using Infrastructure.Database;

namespace SHG.Infrastructure;

public static class InfrastructureHandler
{
    public static async Task InitDbContext(MessagesDbContext dbContext, IServiceProvider serviceProvider)
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
