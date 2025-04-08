using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Repositories;
using Spendo_Backend.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 8080);
});


// Add services to the container.
try
{
    builder.Services.AddDbContext<SpendoContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}
catch (Exception ex)
{
    Console.WriteLine($"Error setting up database context: {ex.Message}");
    throw;
}
builder.Services.AddControllers();
builder.Services.AddScoped<ISpendoRepository, SpendoRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
);
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();


