using MyForeignCards.Endpoints;
using MyForeignCards.Models;
using MyForeignCards.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WordService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapWordEndpoints();

app.Run();