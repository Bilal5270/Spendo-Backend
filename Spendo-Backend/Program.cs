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


var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var db = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var pw = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pw}";

builder.Services.AddDbContext<SpendoContext>(options =>
    options.UseNpgsql(connectionString));
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

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
);
