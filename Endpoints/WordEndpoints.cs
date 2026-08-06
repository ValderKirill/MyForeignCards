using MyForeignCards.Models;
using MyForeignCards.Services;

namespace MyForeignCards.Endpoints
{
    public static class WordEndpoints
    {
        public static void MapWordEndpoints(this WebApplication app)
        {
            app.MapGet("/api/words", async (HttpResponse response, WordService wordService) =>
            {
                return Results.Json(wordService.Words());
            });

            app.MapGet("/api/words/{id:guid}", async (Guid id, HttpResponse response, WordService wordService) =>
            {
                var word = wordService.GetWordById(id);

                if (word != null)
                {
                    return Results.Json(word);
                }
                else
                {
                    return Results.NotFound("Не нашли искомое слово");
                }
            });

            app.MapPost("/api/words", async (HttpResponse response, WordModel newWord, WordService wordService) =>
            {
                if (newWord is not null &&
                    !string.IsNullOrWhiteSpace(newWord.Word) &&
                    !string.IsNullOrWhiteSpace(newWord.Translation))
                {
                    wordService.AddWord(newWord);
                    return Results.Json(newWord, statusCode: 200);
                }
                else
                {
                    return Results.BadRequest("Не смогли добавить пустое слово!");
                }
            });

            app.MapDelete("/api/words/{id:guid}", async (Guid id, HttpResponse response, WordService wordService) =>
            {
                var result = wordService.DeleteWordById(id);

                if (result)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.NotFound("Не нашли слово с нужным ID!");
                }
            });

            app.MapPut("/api/words/{id:guid}", async (Guid id, WordModel word, HttpResponse response, WordService wordService) =>
            {
                if (word != null)
                {
                    var result = wordService.ChangeWord(id, word);

                    if (result)
                    {
                        return Results.Json(word, statusCode: 200);
                    }
                    else
                    {
                        return Results.NotFound("Не найдено слово с указанным id");
                    }
                }
                else
                {
                    return Results.BadRequest("Пустые входные данные");
                }
            });
        }
    }
}
