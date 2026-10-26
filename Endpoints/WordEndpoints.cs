using Microsoft.EntityFrameworkCore;
using MyForeignCards.DTOs;
using MyForeignCards.Entities;
using MyForeignCards.Services;
using MyForeignCards.Utils;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app)
        {
            app.MapGet("/api/words", async (WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                var words = await wordService.GetAllWordsAsync(userId);
                return Results.Ok(words);
            }).RequireAuthorization();

            app.MapGet("/api/words/{wordId:guid}", async (Guid wordId, WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                var word = await wordService.GetWordByIdAsync(wordId, userId);

                if (word is null)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли искомое слово"
                    });
                }

                return Results.Ok(word);
            }).RequireAuthorization();

            app.MapGet("/api/words/category/{categoryId:guid}", async (Guid categoryId, WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                var words = await wordService.GetWordsByCategoryAsync(categoryId, userId);

                return Results.Ok(words);
            }).RequireAuthorization();

            app.MapPost("/api/words", async (WordRequest wordReq, WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                if (wordReq is null ||
                    string.IsNullOrWhiteSpace(wordReq.Text) ||
                    string.IsNullOrWhiteSpace(wordReq.Translation))
                {
                    return Results.BadRequest(new
                    {
                        message = "Не смогли добавить пустое слово!"
                    });
                }

                var word = new Word
                {
                    Text = wordReq.Text,
                    Translation = wordReq.Translation,
                    UserId = userId
                };

                if (wordReq.CategoryId != null)
                {
                    word.CategoryId = wordReq.CategoryId;
                }

                var result = await wordService.AddWordAsync(word);

                if (result == null)
                {
                    return Results.NotFound(new
                    {
                        message = "Категория недоступна текущему пользователю"
                    });
                }

                return Results.Created($"/api/words/{result.Id}", result);
            }).RequireAuthorization();

            app.MapDelete("/api/words/{id:guid}", async (Guid id, WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                var result = await wordService.DeleteWordByIdAsync(id, userId);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли слово с нужным ID!"
                    });
                }

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapPut("/api/words/{id:guid}", async (Guid id, WordRequest wordReq, WordService wordService, HttpContext context) =>
            {
                if (!UserUtils.GetId(context.User, out Guid userId))
                {
                    return Results.Unauthorized();
                }

                if (wordReq is null ||
                    string.IsNullOrWhiteSpace(wordReq.Text) ||
                    string.IsNullOrWhiteSpace(wordReq.Translation))
                {
                    return Results.BadRequest(new
                    {
                        message = "Пустые входные данные"
                    });
                }

                var word = new Word()
                {
                    Id = id,
                    Text = wordReq.Text,
                    Translation = wordReq.Translation,
                    CategoryId = wordReq.CategoryId
                };

                var result = await wordService.ChangeWordAsync(id, userId, word);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Не найдено слово с указанным id"
                    });
                }

                return Results.Ok(new WordResponse
                {
                    Id = word.Id,
                    Text = word.Text,
                    Translation = word.Translation,
                    CategoryId = word.CategoryId
                });
            }).RequireAuthorization();
        }
    }
}
