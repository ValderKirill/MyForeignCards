using MyForeignCards.DTOs;
using MyForeignCards.Entities;
using MyForeignCards.Services;
using MyForeignCards.Utils;

namespace MyForeignCards.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            app.MapGet("/api/categories", async (CategoryService categoryService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                var categories = await categoryService.GetAllCategoriesAsync(userId);
                return Results.Ok(categories);
            }).RequireAuthorization();

            app.MapPost("/api/categories", async (CategoryRequest categoryReq, CategoryService categoryService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                if (categoryReq is null || 
                    string.IsNullOrWhiteSpace(categoryReq.Name))
                {
                    return Results.BadRequest(new
                    {
                        message = "Не смогли добавть пустую категорию"
                    });
                }

                var category = new Category
                {
                    Name = categoryReq.Name,
                    UserId = userId
                };

                var result = await categoryService.AddCategoryAsync(category);
                return Results.Created($"/api/categories/{result.Id}", result);
            }).RequireAuthorization();

            app.MapDelete("/api/categories/{id:guid}", async (Guid id, CategoryService categoryService) =>
            {
                var result = await categoryService.DeleteCategoryByIdAsync(id);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Category not found!"
                    });
                }

                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}
