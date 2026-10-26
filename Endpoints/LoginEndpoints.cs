using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyForeignCards.DTOs;
using MyForeignCards.Entities;
using MyForeignCards.Services;
using System.Security.Claims;

namespace MyForeignCards.Endpoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(this WebApplication app)
        {
            app.MapPost("/login", async (UserRequest user,
                HttpContext context,
                AuthService service) =>
            {
                var currentUser = await service.GetUserAsync(user);

                if (currentUser is null)
                {
                    return Results.Unauthorized();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, currentUser.Login)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Results.Ok();
            });

            app.MapPost("/registration", async (UserRequest userReq, HttpContext context, AuthService service) =>
            {
                if (userReq is null ||
                    string.IsNullOrWhiteSpace(userReq.Login) ||
                    string.IsNullOrWhiteSpace(userReq.Password))
                {
                    return Results.BadRequest(new
                    {
                        message = "Не смогли добавить пустого пользователя!"
                    });
                }

                var user = new User
                {
                    Login = userReq.Login,
                    Password = userReq.Password,
                };

                var result = await service.AddUserAsync(user);

                if(result is null)
                {
                    return Results.BadRequest(new
                    {
                        message = "Пользователь с таким именем уже существует"
                    });
                }

                return Results.Created((string?)null, result);
            });

            app.MapPost("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Ok();
            });
        }
    }
}
