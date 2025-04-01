using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Repositories;
using Spendo_Backend.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Set Kestrel to listen on all network interfaces on port 8080
builder.WebHost.UseUrls("http://0.0.0.0:8523");

// Add services to the container.
builder.Services.AddDbContext<SpendoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<ISpendoRepository, SpendoRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Allow CORS for all origins and headers.
app.UseCors(options => options.AllowAnyHeader().AllowAnyOrigin());

// Remove HTTPS redirection since HTTPS termination is handled by Coolify.
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
