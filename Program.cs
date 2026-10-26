using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.Endpoints;
using MyForeignCards.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string is not found");

builder.Services.AddDbContext<ApplicationContext>(
    options => options
        .UseNpgsql(connection)
        .UseSnakeCaseNamingConvention());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => option.LoginPath = "/login.html");

builder.Services.AddAuthorization();

builder.Services.AddScoped<WordService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("login.html");

app.UseDefaultFiles(options);
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapErrorEndpoints();
app.MapWordEndpoints();
app.MapCategoryEndpoints();
app.MapLoginEndpoints();

app.Run();