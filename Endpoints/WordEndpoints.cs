using MyForeignCards.Models;
using MyForeignCards.Services;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app)
        {
            app.MapGet("/api/words", (WordService wordService) =>
            {
                return Results.Ok(wordService.Words());
            });

            app.MapGet("/api/words/{id:guid}", (Guid id, WordService wordService) =>
            {
                var word = wordService.GetWordById(id);

                if (word == null)
                {
                    return Results.NotFound(new
                    {
                        message = "Не нашли искомое слово"
                    });
                }

                return Results.Ok(word);
            });

            app.MapPost("/api/words", (WordModel newWord, WordService wordService) =>
            {
                if (newWord is null ||
                    string.IsNullOrWhiteSpace(newWord.Word) ||
                    string.IsNullOrWhiteSpace(newWord.Translation))
                {
                    return Results.BadRequest(new
                    {
                        message = "Не смогли добавить пустое слово!"
                    });
                }

                wordService.AddWord(newWord);
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

            app.MapPut("/api/words/{id:guid}", (Guid id, WordModel word, WordService wordService) =>
            {
                if (word == null)
                {
                    return Results.BadRequest(new
                    {
                        message = "Пустые входные данные"
                    });
                    
                }

                var result = wordService.ChangeWord(id, word);

                if (result)
                {
                    return Results.Ok(word);
                }
                else
                {
                    return Results.NotFound(new
                    {
                        message = "Не найдено слово с указанным id"
                    });
                }
            });
        }
    }
}
