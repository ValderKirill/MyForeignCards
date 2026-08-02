using MyForeignCards.Endpoints;
using MyForeignCards.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WordService>();

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