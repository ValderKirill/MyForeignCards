using MyForeignCards.DTOs;
using MyForeignCards.Entities;
using MyForeignCards.Services;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app)
        {
            app.MapGet("/api/words", async (WordService wordService) =>
            {
                var words = await wordService.GetAllWordsAsync();
                return Results.Ok(words);
            });

            app.MapGet("/api/words/{id:guid}", async (Guid id, WordService wordService) =>
            {
                var word = await wordService.GetWordByIdAsync(id);

                if (word is null)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли искомое слово"
                    });
                }

                return Results.Ok(word);
            });

            app.MapPost("/api/words", async (WordRequest wordReq, WordService wordService) =>
            {
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
                    Translation = wordReq.Translation
                };

                var result = await wordService.AddWordAsync(word);
                return Results.Created($"/api/words/{result.Id}", result);
            });

            app.MapDelete("/api/words/{id:guid}", async (Guid id, WordService wordService) =>
            {
                var result = await wordService.DeleteWordByIdAsync(id);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли слово с нужным ID!"
                    });
                }

                return Results.NoContent();
            });

            app.MapPut("/api/words/{id:guid}", async (Guid id, WordRequest wordReq, WordService wordService) =>
            {
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
                    Translation = wordReq.Translation
                };

                var result = await wordService.ChangeWordAsync(id, word);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Не найдено слово с указанным id"
                    });
                }

                return Results.Ok(word);
            });
        }
    }
}
