using Infrastructure;
using Infrastructure.Database;
using SHG.Infrastructure;

namespace ChatAppWebApi
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
            await InfrastructureHandler.InitDbContext(scope.ServiceProvider.GetRequiredService<MessagesDbContext>(), scope.ServiceProvider);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
