using Microsoft.AspNetCore.Diagnostics;

namespace MyForeignCards.Endpoints
{
    public static class ErrorEndpoints
    {
        public static void MapErrorEndpoints(this WebApplication app)
        {
            app.Map("/error", (HttpContext context, ILogger<Program> logger) =>
            {
                var exceptionFeature = context.Features
                    .Get<IExceptionHandlerFeature>();

                if (exceptionFeature?.Error is not null)
                {
                    logger.LogError(
                        exceptionFeature.Error,
                        "Unhandled exception occurred");
                }

                return Results.Problem(
                    title: "An unexpected error occurred",
                    statusCode: StatusCodes.Status500InternalServerError);
            });
        }
    }
}
