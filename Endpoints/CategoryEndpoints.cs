using MyForeignCards.DTOs;
using MyForeignCards.Entities;
using MyForeignCards.Services;

namespace MyForeignCards.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            app.MapPost("/api/categories", async (CategoryRequest categoryReq, CategoryService categoryService) =>
            {
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
                    Name = categoryReq.Name
                };

                var result = await categoryService.AddCategoryAsync(category);
                return Results.Created($"/api/categories/{result.Id}", result);
            });

            app.MapGet("/api/categories", async (CategoryService categoryService) =>
            {
                var categories = await categoryService.GetAllCategoriesAsync();
                return Results.Ok(categories);
            });

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
            });
        }
    }
}
