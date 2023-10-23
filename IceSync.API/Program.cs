using Refit;
using IceSync.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using IceSync.Application.Configuration;
using IceSync.API.Middleware;
using IceSync.Persistance.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.Configure<UniLoaderApiConfiguration>(builder.Configuration.GetSection(nameof(UniLoaderApiConfiguration)));

builder
    .Services
    .AddApplicationServices()
    .AddInfrastructureServices();

builder.Services.ConfigureRefitClients(builder.Configuration);

builder
    .Services
    .AddDatabase(builder.Configuration)
    .AddRepositories();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RefitExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Initialize();

app.Run();
