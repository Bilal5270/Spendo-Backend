using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Repositories;
using Spendo_Backend.Services;
using System.Net;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 8080);
});


DotNetEnv.Env.Load();

var host = Environment.GetEnvironmentVariable("DB_HOST");
var internalPort = Environment.GetEnvironmentVariable("DB_PORT");
var db = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var pw = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Log the loaded values to confirm they are correct
Console.WriteLine($"DB_HOST: {host}");
Console.WriteLine($"DB_PORT: {internalPort}");
Console.WriteLine($"DB_NAME: {db}");
Console.WriteLine($"DB_USER: {user}");
// Be cautious with logging sensitive info like passwords in production!

var connectionString = $"Host={host};Port={internalPort};Database={db};Username={user};Password={pw}";

builder.Services.AddDbContext<SpendoContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddControllers();
builder.Services.AddScoped<ISpendoRepository, SpendoRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spendo API V1");
});


app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();


