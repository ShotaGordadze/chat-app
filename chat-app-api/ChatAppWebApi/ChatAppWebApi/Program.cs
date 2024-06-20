using Application;
using Application.RabbitMQ;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Connections;
using SHG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(config);
builder.Services.AddApplication(config);

builder.Services.AddSingleton<RabbitMQPersistentConnection>();
builder.Services.AddSingleton<IMessageService, MessageService>();

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
await InfrastructureHandler.InitDbContext(scope.ServiceProvider.GetRequiredService<MessagesDbContext>(), scope.ServiceProvider);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
