using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.Endpoints;
using MyForeignCards.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string is not found");

builder.Services.AddDbContext<ApplicationContext>(
    options => options
        .UseNpgsql(connection)
        .UseSnakeCaseNamingConvention());
builder.Services.AddScoped<WordService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapErrorEndpoints();
app.MapWordEndpoints();

app.Run();