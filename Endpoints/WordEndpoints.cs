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
                var words = await wordService.GetAllWords();
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

            app.MapPost("/api/words", (Word newWord, WordService wordService) =>
            {
                if (newWord is null ||
                    string.IsNullOrWhiteSpace(newWord.Text) ||
                    string.IsNullOrWhiteSpace(newWord.Translation))
                {
                    return Results.BadRequest(new
                    {
                        message = "Не смогли добавить пустое слово!"
                    });
                }

                wordService.AddWordAsync(newWord);
                return Results.Created($"/api/words/{newWord.Id}", newWord);
            });

            app.MapDelete("/api/words/{id:guid}", (Guid id, WordService wordService) =>
            {
                var result = wordService.DeleteWordById(id);

                if (!result)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли слово с нужным ID!"
                    });
                }

                return Results.NoContent();
            });

            app.MapPut("/api/words/{id:guid}", (Guid id, Word word, WordService wordService) =>
            {
                if (word is null)
                {
                    return Results.BadRequest(new
                    {
                        message = "Пустые входные данные"
                    });
                }

                var result = wordService.ChangeWord(id, word);

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
